using System.Threading.Tasks;
using AutoMapper;
using FirstAPI.Interfaces;
using FirstAPI.Misc;
using FirstAPI.Models;
using FirstAPI.Models.DTOs;
using System.Security.Claims;
using FirstAPI.Repositories;


namespace FirstAPI.Services
{
    public class AppointmentService : IAppointmentService{
        private readonly IRepository<string, Appointment> _appointmentRepository;
        private readonly DoctorRepository _doctorRepository;

        public AppointmentService(IRepository<string, Appointment> appointmentRepository,
                                  DoctorRepository doctorRepository) {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
        }

        private string GenerateAppointmentNumber()
        {
            return $"APT-{DateTime.UtcNow.Ticks}";
        }

        public async Task<Appointment> BookAppointment(AppointmentAddRequestDto appointmentDto)
        {
            try
            {
                var appointment = new Appointment
                {
                    AppointmentNumber = GenerateAppointmentNumber(),
                    PatientId = appointmentDto.PatientId,
                    DoctorId = appointmentDto.DoctorId,
                    AppointmentDateTime = DateTime.UtcNow,
                    Status = appointmentDto.Status
                };

                var createdAppointment = await _appointmentRepository.Add(appointment);
                return createdAppointment;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Appointment?> GetAppointmentByIdAsync(string appointmentNo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(appointmentNo))
                    return null;

                var appointment = await _appointmentRepository.Get(appointmentNo.Trim());

                return appointment; 
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while fetching appointment: {ex.Message}");
            }
        }



        public async Task<bool> UpdateAppointmentStatusAsync(string appointmentNo, string newStatus)
        {
            try
            {
                var existingAppointment = await _appointmentRepository.Get(appointmentNo);
                if (existingAppointment == null)
                    return false;

                // Update only the status
                existingAppointment.Status = newStatus;

                await _appointmentRepository.Update(appointmentNo, existingAppointment);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while updating appointment status: {ex.Message}");
            }
        }

        public async Task<bool> CancelAppointment(string appointmentNo, ClaimsPrincipal user)
        {
                // Get logged-in user's email
                var email = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(email))
                    throw new UnauthorizedAccessException("User is not authenticated");

                // Get doctor entity using email
                var doctor = await _doctorRepository.GetDoctorByEmailAsync(email);
                if (doctor == null)
                    throw new Exception("Doctor is not valid");

                // Get appointment
                var appointment = await _appointmentRepository.Get(appointmentNo);
                if (appointment == null)
                    throw new KeyNotFoundException("Appointment not found");

                // Check ownership
                if (appointment.DoctorId != doctor.Id)
                    throw new UnauthorizedAccessException("You are not authorized to cancel this appointment");

                // Cancel the appointment
                appointment.Status = "Cancelled";
                await UpdateAppointmentStatusAsync(appointment.AppointmentNumber, "Cancelled");

                return true;
           
        }




    }
}