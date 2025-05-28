using FirstAPI.Interfaces;
using FirstAPI.Models;
using FirstAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FirstAPI.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IRepository<int, Doctor> _doctorRepo;
        private readonly IRepository<int, Speciality> _specialityRepo;
        private readonly IRepository<int, DoctorSpeciality> _doctorSpecialityRepo;

        public DoctorService(IRepository<int, Doctor> doctorRepo,
                             IRepository<int, Speciality> specialityRepo,
                             IRepository<int, DoctorSpeciality> doctorSpecialityRepo)
        {
            _doctorRepo = doctorRepo;
            _specialityRepo = specialityRepo;
            _doctorSpecialityRepo = doctorSpecialityRepo;
        }


        public async Task<Doctor> GetDoctByName(string name)
        {
            var doctors = await _doctorRepo.GetAll();
            var doctor = doctors.FirstOrDefault(d => d.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return doctor ?? throw new Exception("Doctor not found with given name.");
        }


        public async Task<ICollection<Doctor>> GetDoctorsBySpeciality(string speciality)
        {
            var allSpecialities = await _specialityRepo.GetAll();
            var matchedSpeciality = allSpecialities.FirstOrDefault(s => s.Name.Equals(speciality, StringComparison.OrdinalIgnoreCase));

            if(matchedSpeciality == null) {
                throw new Exception("Speciality not Found!");
            }

            var allDoctorSpecialities = await _doctorSpecialityRepo.GetAll();
            var doctorIds = allDoctorSpecialities
                        .Where(ds => ds.SpecialityId == matchedSpeciality.Id)
                        .Select(ds => ds.DoctorId)
                        .ToList();

            var allDoctors = await _doctorRepo.GetAll();
            return allDoctors.Where(d => doctorIds.Contains(d.Id)).ToList();
        }

        

        public async Task<Doctor> AddDoctor(DoctorAddRequestDto doctorDto)
        {
            var doctor = new Doctor
            {
                Name = doctorDto.Name,
                YearsOfExperience = doctorDto.YearsOfExperience,
                Status = "Active"
            };
            var addedDoctor = await _doctorRepo.Add(doctor);

            if (doctorDto.Specialities != null)
            {
                var allSpecialities = await _specialityRepo.GetAll();
                foreach (var specDto in doctorDto.Specialities)
                {
                    var speciality = allSpecialities.FirstOrDefault(s => s.Name.Equals(specDto.Name, StringComparison.OrdinalIgnoreCase));
                    if (speciality == null)
                    {
                        // Add new Speciality if it doesnt exist
                        speciality = await _specialityRepo.Add(new Speciality { 
                                                                    Name = specDto.Name,
                                                                    Status = "Active"
                                                                });
                    }
                   // To Link Doctor and Speciality
                    await _doctorSpecialityRepo.Add(new DoctorSpeciality
                    {
                        DoctorId = addedDoctor.Id,
                        SpecialityId = speciality.Id
                    });
                }
            }
            return addedDoctor;
        }
    }
}
