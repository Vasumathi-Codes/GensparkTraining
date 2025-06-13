using AutoMapper;
using LeaveManagementSystem.Interfaces;
using LeaveManagementSystem.Models;
using LeaveManagementSystem.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;


namespace LeaveManagementSystem.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly IRepository<Guid, LeaveRequest> _leaveRequestRepository;
        private readonly ILeaveBalanceService _leaveBalanceService;
        private readonly IMapper _mapper;
        private readonly IAuditLogService _auditLogService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserService _userService;
        private readonly ILeaveTypeService _leaveTypeService;

        public LeaveRequestService(
            IRepository<Guid, LeaveRequest> leaveRequestRepository,
            ILeaveBalanceService leaveBalanceService,
            IMapper mapper,
            IAuditLogService auditLogService,
            ICurrentUserService currentUserService,
            IUserService userService,
            ILeaveTypeService leaveTypeService)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _leaveBalanceService = leaveBalanceService;
            _mapper = mapper;
            _auditLogService = auditLogService;
            _currentUserService = currentUserService;
            _userService = userService;
            _leaveTypeService = leaveTypeService;
        }

        public async Task<IEnumerable<LeaveRequestResponseDto>> GetAllAsync()
        {
            try
            {
                var requests = await _leaveRequestRepository.GetAll();
                var currentUserId = _currentUserService.GetUserId();
                var currentUserRole = _currentUserService.GetRole();

                // If the user is not HR, filter leave requests to only their own
                if (currentUserRole != "HR") 
                {
                    requests = requests.Where(r => r.UserId == currentUserId);
                }
                return _mapper.Map<IEnumerable<LeaveRequestResponseDto>>(requests);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get all leave requests", ex);
            }
        }

        public async Task<LeaveRequestResponseDto> GetByIdAsync(Guid id)
        {
            try
            {
                var request = await _leaveRequestRepository.Get(id);
                if (request == null)
                    throw new KeyNotFoundException($"Leave request with ID {id} not found");

                return _mapper.Map<LeaveRequestResponseDto>(request);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get leave request by ID", ex);
            }
        }

        //---------------------------CREATE LEAVE REQUEST with VALIDATIONS--------------------------------

        public async Task<LeaveRequestResponseDto> CreateAsync(LeaveRequestDto dto)
        {
            try
            {
                await ValidateGenderBasedLeaveAsync(dto.UserId, dto.LeaveTypeId);
                ValidateStartDate(dto.StartDate);
                await ValidateOverlappingLeaveAsync(dto);
                int requestedDays = GetAndValidateRequestedDays(dto.StartDate, dto.EndDate);
                
                var leaveType = await GetAndValidateLeaveType(dto.LeaveTypeId);

                if (!IsLeaveTypeExemptedFromBalanceCheck(leaveType.Name))
                {
                    await ValidateLeaveBalanceAsync(dto, requestedDays, leaveType);
                    await ValidateMonthlyLimitAsync(dto, requestedDays, leaveType);
                }

                var entity = await CreateLeaveRequestEntityAsync(dto);

                await _auditLogService.LogAsync(
                    _currentUserService.GetUserId(),
                    "INSERTED",
                    "LEAVEREQUEST",
                    entity.Id
                );

                entity = await _leaveRequestRepository.Get(entity.Id);
                return _mapper.Map<LeaveRequestResponseDto>(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create leave request {ex.Message}", ex);
            }
        }


        private void ValidateStartDate(DateTime startDate)
        {
            if (startDate.Date < DateTime.UtcNow.Date)
                throw new InvalidOperationException("Leave start date cannot be in the past.");
        }

        private async Task ValidateOverlappingLeaveAsync(LeaveRequestDto dto)
        {
            var existingLeaves = await _leaveRequestRepository.GetAll();
            bool hasOverlap = existingLeaves.Any(l =>
                l.UserId == dto.UserId &&
                l.Status != "Rejected" &&
                l.StartDate <= dto.EndDate &&
                l.EndDate >= dto.StartDate);

            if (hasOverlap)
                throw new InvalidOperationException("You have already applied for leave in this date range.");
        }

        private int GetAndValidateRequestedDays(DateTime startDate, DateTime endDate)
        {
            int requestedDays = GetWorkingDays(startDate, endDate);
            if (requestedDays > 15)
                throw new InvalidOperationException("You cannot apply for more than 15 consecutive leave days.");
            return requestedDays;
        }

        private async Task<LeaveType> GetAndValidateLeaveType(Guid leaveTypeId)
        {
            var leaveType = await _leaveTypeService.GetByIdAsync(leaveTypeId);
            if (leaveType == null)
                throw new InvalidOperationException("Leave type not found.");
            return leaveType;
        }

        private bool IsLeaveTypeExemptedFromBalanceCheck(string leaveTypeName)
        {
            return leaveTypeName.Contains("Sick", StringComparison.OrdinalIgnoreCase) ||
                leaveTypeName.Contains("Loss Of Pay", StringComparison.OrdinalIgnoreCase);
        }

        private async Task ValidateLeaveBalanceAsync(LeaveRequestDto dto, int requestedDays, LeaveType leaveType)
        {
            var leaveBalanceResponse = await _leaveBalanceService.GetLeaveBalanceForTypeAsync(dto.UserId, dto.LeaveTypeId);
            if (leaveBalanceResponse == null || leaveBalanceResponse.LeaveBalance == null)
                throw new InvalidOperationException("Leave balance not found for this leave type.");

            int remainingDays = leaveBalanceResponse.LeaveBalance.RemainingLeaves;
            if (requestedDays > remainingDays)
                throw new InvalidOperationException($"Insufficient leave balance. Available days: {remainingDays}");
        }

        private async Task ValidateMonthlyLimitAsync(LeaveRequestDto dto, int requestedDays, LeaveType leaveType)
        {
            int monthlyLimit = leaveType.StandardLeaveCount / 12;

            var allRequests = await _leaveRequestRepository.GetAll();
            var usedThisMonth = allRequests
                .Where(lr => lr.UserId == dto.UserId &&
                            lr.LeaveTypeId == dto.LeaveTypeId &&
                            lr.StartDate.Month == dto.StartDate.Month &&
                            lr.StartDate.Year == dto.StartDate.Year &&
                            lr.Status == "Approved")
                .Sum(lr => GetWorkingDays(lr.StartDate, lr.EndDate));

            if ((usedThisMonth + requestedDays) > monthlyLimit)
                throw new InvalidOperationException(
                    $"Monthly leave limit exceeded. Allowed: {monthlyLimit} days, Used: {usedThisMonth} days, Requested: {requestedDays} days.");
        }

        private async Task<LeaveRequest> CreateLeaveRequestEntityAsync(LeaveRequestDto dto)
        {
            var entity = _mapper.Map<LeaveRequest>(dto);
            entity = await _leaveRequestRepository.Add(entity);
            return entity;
        }


        //---------------------------------------------------------------------------------------

    
        public async Task<bool> UpdateStatusAsync(Guid id, string status, Guid approverId)
        {
            try
            {
                var request = await _leaveRequestRepository.Get(id);
                if (request == null)
                    throw new KeyNotFoundException($"Leave request with ID {id} not found");

                if (request.Status == "Approved" || request.Status == "Rejected")
                    throw new InvalidOperationException("Cannot change status of a finalized request.");
                
                if (request.UserId == approverId)
                    throw new InvalidOperationException("You cannot approve or reject your own leave request.");

                request.Status = status;
                request.ReviewedById = approverId;
                request.UpdatedAt = DateTime.UtcNow;

                if (status == "Approved")
                {
                    int requestedDays = GetWorkingDays(request.StartDate, request.EndDate);

                    await _leaveBalanceService.DeductLeaveBalanceAsync(
                        request.UserId,
                        request.LeaveTypeId,
                        requestedDays
                    );
                }

                await _leaveRequestRepository.Update(id, request);

                await _auditLogService.LogAsync(
                    _currentUserService.GetUserId(),
                    "UPDATED",
                    "LEAVEREQUEST",
                    request.Id
                );
                
                return true;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update leave request status {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var deletedItem = await _leaveRequestRepository.Delete(id);

                await _auditLogService.LogAsync(
                    _currentUserService.GetUserId(),
                    "DELETED",
                    "LEAVEREQUEST",
                    deletedItem.Id
                );

                return true;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete leave request", ex);
            }
        }

        private async Task ValidateGenderBasedLeaveAsync(Guid userId, Guid leaveTypeId)
        {
            var user = await _userService.GetUserById(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found.");

            var leaveType = await _leaveTypeService.GetByIdAsync(leaveTypeId);
            if (leaveType == null)
                throw new KeyNotFoundException($"Leave Type with ID {leaveTypeId} not found.");

            var gender = user.Gender?.ToLower();
            var leaveTypeName = leaveType.Name?.ToLower();

            if (leaveTypeName.Contains("maternity") && gender != "female")
                throw new InvalidOperationException("Only female employees can apply for Maternity Leave.");

            if (leaveTypeName.Contains("paternity") && gender != "male")
                throw new InvalidOperationException("Only male employees can apply for Paternity Leave.");
        }

        private int GetWorkingDays(DateTime start, DateTime end)
        {
            int totalDays = 0;

            for (DateTime date = start; date <= end; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    totalDays++;
                }
            }

            return totalDays;
        }
    }
}
