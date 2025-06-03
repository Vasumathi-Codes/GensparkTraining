using FirstAPI.Models.DTOs;
using FirstAPI.Models;
using System.Security.Claims;

namespace FirstAPI.Interfaces {
    public interface IAppointmentService
    {
        public Task<Appointment?> GetAppointmentByIdAsync(string appointmentNo);
        public Task<Appointment> BookAppointment(AppointmentAddRequestDto appointmentDto);
        public Task<bool> CancelAppointment(string appointmentNo, ClaimsPrincipal User);
    }

}