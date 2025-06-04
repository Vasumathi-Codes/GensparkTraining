using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FirstAPI.Interfaces;
using FirstAPI.Misc;
using FirstAPI.Models;
using FirstAPI.Models.DTOs;
using FirstAPI.Services;
using FirstAPI.Contexts;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
 
namespace FirstAPI.Test
{
    public class PatientServiceTests
    {
        private Mock<IRepository<int, Patient>> _patientRepositoryMock;
        private Mock<IRepository<string, User>> _userRepositoryMock;
        private Mock<IEncryptionService> _encryptionServiceMock;
        private Mock<IMapper> _mapperMock;
 
        private PatientService _service;
 
        [SetUp]
        public void Setup()
        {
            _patientRepositoryMock = new Mock<IRepository<int, Patient>>();
            _userRepositoryMock = new Mock<IRepository<string, User>>();
            _encryptionServiceMock = new Mock<IEncryptionService>();
            _mapperMock = new Mock<IMapper>();
 
            _service = new PatientService(
                _mapperMock.Object,
                _encryptionServiceMock.Object,
                _userRepositoryMock.Object,
                _patientRepositoryMock.Object
            );
        }
 
        [Test]
        public async Task AddPatient_ShouldReturnPatient_WhenSuccess()
        {
            // Arrange
            var patientDto = new PatientAddRequestDto
            {
                Name = "Test Patient",
                Email = "test@pat.com",
                Password = "secret"
            };
 
            var encrypted = new EncryptModel
            {
                EncryptedData = new byte[] { 1, 2, 3 },
                HashKey = new byte[] { 4, 5, 6 }
            };
 
            var user = new User
            {
                Username = patientDto.Email,
                Password = encrypted.EncryptedData,
                HashKey = encrypted.HashKey,
                Role = "Patient"
            };
 
            var patient = new Patient
            {
                Id = 1,
                Name = patientDto.Name,
                Email = patientDto.Email
            };
 
            _mapperMock.Setup(m => m.Map<PatientAddRequestDto, User>(patientDto)).Returns(user);
            _mapperMock.Setup(m => m.Map<Patient>(patientDto)).Returns(patient);
            _encryptionServiceMock.Setup(e => e.EncryptData(It.IsAny<EncryptModel>())).ReturnsAsync(encrypted);
            _userRepositoryMock.Setup(u => u.Add(It.IsAny<User>())).ReturnsAsync(user);
            _patientRepositoryMock.Setup(p => p.Add(It.IsAny<Patient>())).ReturnsAsync(patient);
 
            // Act
            var result = await _service.AddPatient(patientDto);
 
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Test Patient"));
            Assert.That(result.Email, Is.EqualTo("test@pat.com"));
 
        }
    }
    
}