using BankApp.DTOs;
using BankApp.Interfaces;
using BankApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApp.Mappers;

namespace BankApp.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<int, User> _userRepository;
        private readonly UserMapper _userMapper;
        public UserService(IRepository<int, User> userRepository)
        {
            _userRepository = userRepository;
            _userMapper = new UserMapper();
        }

        public async Task<UserDto> RegisterUser(CreateUserDto userDto)
        {
            try
            {
                var user = _userMapper.MapCreateUserDtoToUser(userDto);
                var createdUser = await _userRepository.Add(user);
                return _userMapper.MapUserToUserDto(createdUser);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error registering user: {ex.Message}");
            }
        }

        public async Task<UserDto> GetUserById(int id)
        {
            try
            {
                var user = await _userRepository.Get(id);
                return _userMapper.MapUserToUserDto(user);
            }
            catch (Exception ex)
            {
                throw new Exception($"User not found: {ex.Message}");
            }
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            try
            {
                var users = await _userRepository.GetAll();
                return _userMapper.MapUserListToUserDtoList(users);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching users: {ex.Message}");
            }
        }

    }
}
