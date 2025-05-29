using System.Threading.Tasks;
using FirstAPI.Interfaces;
using FirstAPI.Misc;
using FirstAPI.Models;
using FirstAPI.Models.DTOs;
using Microsoft.VisualBasic;

namespace FirstAPI.Services
{
    public class DoctorService : IDoctorService
    {
        DoctorMapper doctorMapper ;
        SpecialityMapper specialityMapper;
        private readonly IRepository<int, Doctor> _doctorRepository;
        private readonly IRepository<int, Speciality> _specialityRepository;
        private readonly IRepository<int, DoctorSpeciality> _doctorSpecialityRepository;
        private readonly IOtherContextFunctionalities _otherContextFunctionalities;

        public DoctorService(IRepository<int, Doctor> doctorRepository,
                            IRepository<int, Speciality> specialityRepository,
                            IRepository<int, DoctorSpeciality> doctorSpecialityRepository,
                            IOtherContextFunctionalities otherContextFunctionalities)
        {
            doctorMapper = new DoctorMapper();
            specialityMapper = new();
            _doctorRepository = doctorRepository;
            _specialityRepository = specialityRepository;
            _doctorSpecialityRepository = doctorSpecialityRepository;
            _otherContextFunctionalities = otherContextFunctionalities;
        }

        public async Task<Doctor> AddDoctor(DoctorAddRequestDto doctor)
        {
            try
            {
                var newDoctor = doctorMapper.MapDoctorAddRequestDoctor(doctor);
                newDoctor = await _doctorRepository.Add(newDoctor);
                if (newDoctor == null)
                    throw new Exception("Could not add doctor");
                if (doctor.Specialities.Count() > 0)
                {
                    int[] specialities = await MapAndAddSpeciality(doctor);
                    for (int i = 0; i < specialities.Length; i++)
                    {
                        var doctorSpeciality = specialityMapper.MapDoctorSpecility(newDoctor.Id, specialities[i]);
                        doctorSpeciality = await _doctorSpecialityRepository.Add(doctorSpeciality);
                    }
                }
                return newDoctor;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }

        private async Task<int[]> MapAndAddSpeciality(DoctorAddRequestDto doctor)
        {
            int[] specialityIds = new int[doctor.Specialities.Count()];
            IEnumerable<Speciality> existingSpecialities = null;
            try
            {
                existingSpecialities = await _specialityRepository.GetAll();
            }
            catch (Exception e)
            {

            }
            int count = 0;
            foreach (var item in doctor.Specialities)
            {
                Speciality speciality = null;
                if (existingSpecialities != null)
                    speciality = existingSpecialities.FirstOrDefault(s => s.Name.ToLower() == item.Name.ToLower());
                if (speciality == null)
                {
                    speciality = specialityMapper.MapSpecialityAddRequestDoctor(item);
                    speciality = await _specialityRepository.Add(speciality);
                }
                specialityIds[count] = speciality.Id;
                count++;
            }
            return specialityIds;
        }

        public Task<Doctor> GetDoctByName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<DoctorsBySpecialityResponseDto>> GetDoctorsBySpeciality(string speciality)
        {
            var result = await _otherContextFunctionalities.GetDoctorsBySpeciality(speciality);
            return result;
        }
    }
}

// using FirstAPI.Interfaces;
// using FirstAPI.Models;
// using FirstAPI.Models.DTOs;
// using Microsoft.EntityFrameworkCore;

// namespace FirstAPI.Services
// {
//     public class DoctorService : IDoctorService
//     {
//         private readonly IRepository<int, Doctor> _doctorRepo;
//         private readonly IRepository<int, Speciality> _specialityRepo;
//         private readonly IRepository<int, DoctorSpeciality> _doctorSpecialityRepo;

//         public DoctorService(IRepository<int, Doctor> doctorRepo,
//                              IRepository<int, Speciality> specialityRepo,
//                              IRepository<int, DoctorSpeciality> doctorSpecialityRepo)
//         {
//             _doctorRepo = doctorRepo;
//             _specialityRepo = specialityRepo;
//             _doctorSpecialityRepo = doctorSpecialityRepo;
//         }


//         public async Task<Doctor> GetDoctByName(string name)
//         {
//             var doctors = await _doctorRepo.GetAll();
//             var doctor = doctors.FirstOrDefault(d => d.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
//             return doctor ?? throw new Exception("Doctor not found with given name.");
//         }


//         public async Task<ICollection<Doctor>> GetDoctorsBySpeciality(string speciality)
//         {
//             var allSpecialities = await _specialityRepo.GetAll();
//             var matchedSpeciality = allSpecialities.FirstOrDefault(s => s.Name.Equals(speciality, StringComparison.OrdinalIgnoreCase));

//             if(matchedSpeciality == null) {
//                 throw new Exception("Speciality not Found!");
//             }

//             var allDoctorSpecialities = await _doctorSpecialityRepo.GetAll();
//             var doctorIds = allDoctorSpecialities
//                         .Where(ds => ds.SpecialityId == matchedSpeciality.Id)
//                         .Select(ds => ds.DoctorId)
//                         .ToList();

//             var allDoctors = await _doctorRepo.GetAll();
//             return allDoctors.Where(d => doctorIds.Contains(d.Id)).ToList();
//         }

        

//         public async Task<Doctor> AddDoctor(DoctorAddRequestDto doctorDto)
//         {
//             var doctor = new Doctor
//             {
//                 Name = doctorDto.Name,
//                 YearsOfExperience = doctorDto.YearsOfExperience,
//                 Status = "Active"
//             };
//             var addedDoctor = await _doctorRepo.Add(doctor);

//             if (doctorDto.Specialities != null)
//             {
//                 var allSpecialities = await _specialityRepo.GetAll();
//                 foreach (var specDto in doctorDto.Specialities)
//                 {
//                     var speciality = allSpecialities.FirstOrDefault(s => s.Name.Equals(specDto.Name, StringComparison.OrdinalIgnoreCase));
//                     if (speciality == null)
//                     {
//                         // Add new Speciality if it doesnt exist
//                         speciality = await _specialityRepo.Add(new Speciality { 
//                                                                     Name = specDto.Name,
//                                                                     Status = "Active"
//                                                                 });
//                     }
//                    // To Link Doctor and Speciality
//                     await _doctorSpecialityRepo.Add(new DoctorSpeciality
//                     {
//                         DoctorId = addedDoctor.Id,
//                         SpecialityId = speciality.Id
//                     });
//                 }
//             }
//             return addedDoctor;
//         }
//     }
// }
