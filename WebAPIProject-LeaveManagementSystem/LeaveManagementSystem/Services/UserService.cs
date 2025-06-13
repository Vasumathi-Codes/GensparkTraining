using LeaveManagementSystem.Interfaces;
using LeaveManagementSystem.Models;
using LeaveManagementSystem.Models.DTOs;
using LeaveManagementSystem.Helpers;
using LeaveManagementSystem.Exceptions;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<Guid, User> _userRepository;
        private readonly IAuditLogService _auditLogService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILeaveBalanceService _leaveBalanceService;
        private readonly IMapper _mapper;

        public UserService(
            IRepository<Guid, User> userRepository, 
            IAuditLogService auditLogService, 
            ICurrentUserService currentUserService, 
            ILeaveBalanceService leaveBalanceService,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _auditLogService = auditLogService;
            _currentUserService = currentUserService;
            _leaveBalanceService = leaveBalanceService;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<UserDto> Users, int TotalCount)> GetAllUsers(
                int pageNumber, int pageSize, string searchTerm, string role, string sortBy, string sortOrder)
        {
            try
            {
                var users = await _userRepository.GetAll();
                var userDtos = _mapper.Map<List<UserDto>>(users);

                // Search
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    userDtos = userDtos.Where(u =>
                        (!string.IsNullOrEmpty(u.Username) && u.Username.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(u.Email) && u.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    ).ToList();
                }

                // Filter
                if (!string.IsNullOrWhiteSpace(role))
                {
                    userDtos = userDtos.Where(u => u.Role.Equals(role, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                // Total count BEFORE pagination
                int totalCount = userDtos.Count;

                // Sort
                userDtos = sortBy?.ToLower() switch
                {
                    "name" => (sortOrder == "desc") ? userDtos.OrderByDescending(u => u.Username).ToList() : userDtos.OrderBy(u => u.Username).ToList(),
                    "email" => (sortOrder == "desc") ? userDtos.OrderByDescending(u => u.Email).ToList() : userDtos.OrderBy(u => u.Email).ToList(),
                    _ => (sortOrder == "desc") ? userDtos.OrderByDescending(u => u.CreatedAt).ToList() : userDtos.OrderBy(u => u.CreatedAt).ToList(),
                };

                // Pagination
                userDtos = userDtos
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return (userDtos, totalCount);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve users.", ex);
            }
        }


        public async Task<UserDto> GetUserById(Guid id)
        {
            try
            {
                var user = await _userRepository.Get(id);
                if (user == null)
                    throw new KeyNotFoundException("User not found.");
                return _mapper.Map<UserDto>(user);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve user with id {id}.", ex);
            }
        }

        public async Task<UserDto> CreateUser(CreateUserDto createUserDto)
        {
            try
            {
                var user = _mapper.Map<User>(createUserDto);
                user.Id = Guid.NewGuid();
                
                user.PasswordHash = PasswordHelper.HashPassword(createUserDto.Password);
                user.CreatedAt = DateTime.UtcNow;
                user.CreatedBy = _currentUserService.GetUserId();

                var createdUser = await _userRepository.Add(user);

                await _leaveBalanceService.InitializeLeaveBalancesForUserAsync(createdUser.Id);

                await _auditLogService.LogAsync(_currentUserService.GetUserId(), "INSERTED", "USER", createdUser.Id);

                return _mapper.Map<UserDto>(createdUser);
            }
            catch (DbUpdateException dbEx)
            {
                if (dbEx.InnerException != null &&
                    dbEx.InnerException.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase))
                {
                    throw new DuplicateKeyException("A user with the same unique field (like email or username) already exists.", dbEx);
                }
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create user.", ex);
            }
        }

        public async Task<UserDto> UpdateUser(Guid id, UpdateUserDto updateUserDto)
        {
            try
            {
                var existingUser = await _userRepository.Get(id);
                if (existingUser == null)
                    throw new KeyNotFoundException("User not found.");

                _mapper.Map(updateUserDto, existingUser);
                var updatedUser = await _userRepository.Update(id, existingUser);

                await _auditLogService.LogAsync(
                    _currentUserService.GetUserId(), 
                    "UPDATED", 
                    "USER", 
                    updatedUser.Id);

                return _mapper.Map<UserDto>(updatedUser);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (DbUpdateException dbEx)
            {
                if (dbEx.InnerException != null &&
                    dbEx.InnerException.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase))
                {
                    throw new DuplicateKeyException("A user with the same unique field (like email or username) already exists.", dbEx);
                }
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update user with id {id} {ex.Message}.", ex);
            }
        }

        public async Task<UserDto> DeleteUser(Guid id)
        {
            try
            {
                var existingUser = await _userRepository.Get(id);
                if (existingUser == null)
                    throw new KeyNotFoundException("User not found.");

                var deletedUser = await _userRepository.Delete(id);

                await _auditLogService.LogAsync(
                    _currentUserService.GetUserId(), 
                    "DELETED", 
                    "USER", 
                    deletedUser.Id
                );

                return _mapper.Map<UserDto>(deletedUser);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete user with id {id}.", ex);
            }
        }
    }
}
