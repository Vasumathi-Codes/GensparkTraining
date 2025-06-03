using FirstAPI.Contexts;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using FirstAPI.Models;
using FirstAPI.Repositories;
using FirstAPI.Interfaces;

namespace FirstAPI.Test;
public class Tests
{
    ClinicContext _context;
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ClinicContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

        _context = new ClinicContext(options);
        
    }

    // [Test]
    // public async Task AddDoctorTest()
    // {
    //     //arrange
    //     var email = " test@gmail.com";
    //     var password = System.Text.Encoding.UTF8.GetBytes("test123");
    //     var key = Guid.NewGuid().ToByteArray();
    //     var user = new User
    //     {
    //         Username = email,
    //         Password = password,
    //         HashKey = key,
    //         Role = "Doctor"
    //     };
    //     _context.Add(user);
    //     await _context.SaveChangesAsync();
    //     var doctor = new Doctor
    //     {
    //         Name = "test",
    //         YearsOfExperience = 2,
    //         Email = email
    //     };
    //     IRepository<int, Doctor> _doctorRepository = new DoctorRepository(_context);
    //     //action
    //     var result = await _doctorRepository.Add(doctor);
    //     //assert
    //     Assert.That(result, Is.Not.Null, "Doctor IS not addeed");
    //     Assert.That(result.Id, Is.EqualTo(1));
    //     Assert.That(result.Email, Is.EqualTo("testt@gmail.com"));
    // }

    // [TestCase(1)]
    // [TestCase(2)]
    // public async Task GetDoctorPassTest(int id)
    // {
    //     //arrange
    //     var email = " test@gmail.com";
    //     var password = System.Text.Encoding.UTF8.GetBytes("test123");
    //     var key = Guid.NewGuid().ToByteArray();
    //     var user = new User
    //     {
    //         Username = email,
    //         Password = password,
    //         HashKey = key,
    //         Role = "Doctor"
    //     };
    //     _context.Add(user);
    //     await _context.SaveChangesAsync();
    //     var doctor = new Doctor
    //     {
    //         Name = "test",
    //         YearsOfExperience = 2,
    //         Email = email
    //     };
    //     IRepository<int, Doctor> _doctorRepository = new DoctorRepository(_context);
    //     //action
    //     await _doctorRepository.Add(doctor);

    //     //action
    //     var result = _doctorRepository.Get(id);
    //     //assert
    //     Assert.That(result.Id, Is.EqualTo(id));

    // }

    [TestCase(3)]
    public async Task GetDoctorExceptionTest(int id)
    {
        // Arrange
        var email = "test@gmail.com";
        var password = System.Text.Encoding.UTF8.GetBytes("test123");
        var key = Guid.NewGuid().ToByteArray();

        var user = new User
        {
            Username = email,
            Password = password,
            HashKey = key,
            Role = "Doctor"
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var doctor = new Doctor
        {
            Name = "test",
            YearsOfExperience = 2,
            Email = email
        };
        IRepository<int, Doctor> doctorRepository = new DoctorRepository(_context);
        await doctorRepository.Add(doctor); // this gets ID = 1

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(async () => await doctorRepository.Get(id));
        Assert.That(ex.Message, Is.EqualTo("No Doctor with the given ID"));
    }



    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }
}