using FirstAPI.Models.DTOs;
using FirstAPI.Models;
using System.Security.Claims;

namespace FirstAPI.Interfaces {
    public interface IAppointmentService
    {
        public Task<Appointment?> GetAppointmentByIdAsync(string appointmentNo);
        public Task<IEnumerable<Appointment>> GetAllAppointments();
        public Task<Appointment> BookAppointment(AppointmentAddRequestDto appointmentDto);
        public Task<Appointment?> UpdateAppointmentStatusAsync(string appointmentNo, string newStatus);
        public Task<Appointment?> CancelAppointment(string appointmentNo, ClaimsPrincipal User);
    }

}