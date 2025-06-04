using FirstAPI.Models;
using FirstAPI.Models.DTOs;

namespace FirstAPI.Interfaces
{
    public interface IDoctorService
    {
        public Task<Doctor> GetDoctorByUsername(string name);
        public Task<Doctor> GetDoctorByEmail(string email);
        public Task<ICollection<DoctorsBySpecialityResponseDto>> GetDoctorsBySpeciality(string speciality);
        public Task<Doctor> AddDoctor(DoctorAddRequestDto doctor);
    }
}