using LeaveManagementSystem.Models;
using LeaveManagementSystem.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Interfaces
{
    public interface ILeaveRequestService
    {
        Task<IEnumerable<LeaveRequestResponseDto>> GetAllAsync();
        Task<LeaveRequestResponseDto> GetByIdAsync(Guid id);
        Task<LeaveRequestResponseDto> CreateAsync(LeaveRequestDto request);
        Task<bool> UpdateStatusAsync(Guid id, string status, Guid approverId);
        Task<bool> DeleteAsync(Guid id);
    }
}
