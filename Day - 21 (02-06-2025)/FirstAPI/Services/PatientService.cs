using System.Threading.Tasks;
using AutoMapper;
using FirstAPI.Interfaces;
using FirstAPI.Misc;
using FirstAPI.Models;
using FirstAPI.Models.DTOs;


namespace FirstAPI.Services
{
    public class PatientService : IPatientService
    {
        private readonly IMapper _mapper;
        private readonly IEncryptionService _encryptionService;
        private readonly IRepository<string, User> _userRepository;
        private readonly IRepository<int, Patient> _patientRepository;

        public PatientService(
            IMapper mapper,
            IEncryptionService encryptionService,
            IRepository<string, User> userRepository,
            IRepository<int, Patient> patientRepository)
        {
            _mapper = mapper;
            _encryptionService = encryptionService;
            _userRepository = userRepository;
            _patientRepository = patientRepository;
        }

        public async Task<Patient> AddPatient(PatientAddRequestDto patient)
        {
            try
            {
                var user = _mapper.Map<PatientAddRequestDto, User>(patient);
                var encryptedData = await _encryptionService.EncryptData(new EncryptModel
                {
                    Data= patient.Password
                });
                user.Password = encryptedData.EncryptedData;
                user.HashKey = encryptedData.HashKey;
                user.Role = "Patient";
                user = await _userRepository.Add(user);
                var newPatient = _mapper.Map<Patient>(patient);
                newPatient = await _patientRepository.Add(newPatient);
                if (newPatient == null)
                    throw new Exception("Could not add patient");
                return newPatient;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }

    }
}