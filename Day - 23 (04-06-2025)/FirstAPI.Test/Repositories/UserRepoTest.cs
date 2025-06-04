using FirstAPI.Contexts;
using FirstAPI.Models;
using FirstAPI.Repositories;
using FirstAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FirstAPI.Test
{
    public class UserTest
    {
        private ClinicContext _context;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ClinicContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ClinicContext(options);
        }

        private User CreateSampleUser(string username = "testuser@example.com")
        {
            return new User
            {
                Username = username,
                Password = new byte[] {1,2,3},
                HashKey = new byte[] {4,5,6},
                Role = "Patient"
            };
        }

        [Test]
        public async Task CreateUser_Succeeds()
        {
            var repository = new UserRepository(_context);
            var user = CreateSampleUser();

            var savedUser = await repository.Add(user);

            Assert.That(savedUser, Is.Not.Null);
            Assert.That(savedUser.Username, Is.EqualTo(user.Username));
        }

        [Test]
        public async Task AddDuplicateUser_ShouldFail()
        {
            var repository = new UserRepository(_context);
            var user = CreateSampleUser("duplicate@user.com");

            await repository.Add(user);

            var ex = Assert.ThrowsAsync<Exception>(async () => await repository.Add(user));

            Assert.That(ex?.Message, Does.Contain("duplicate").IgnoreCase);
        }

        [Test]
        public async Task GetUser_ReturnsCorrectUser()
        {
            var repository = new UserRepository(_context);
            var user = CreateSampleUser();
            await repository.Add(user);

            var retrieved = await repository.Get(user.Username);

            Assert.That(retrieved.Username, Is.EqualTo(user.Username));
        }

        [Test]
        public void GetUser_InvalidKey_ThrowsException()
        {
            var repository = new UserRepository(_context);

            var ex = Assert.ThrowsAsync<Exception>(async () => await repository.Get("a@email.com"));
            Assert.That(ex.Message, Is.EqualTo("No User with the given ID"));
        }

        [Test]
        public async Task GetAllUsers_ReturnsExpectedCount()
        {
            var repository = new UserRepository(_context);
            var users = new[]
            {
                CreateSampleUser("a@example.com"),
                CreateSampleUser("b@example.com"),
                CreateSampleUser("c@example.com")
            };

            foreach (var user in users)
                await repository.Add(user);

            var allUsers = (await repository.GetAll()).ToList();

            Assert.That(allUsers.Count, Is.EqualTo(users.Length));
        }

        [Test]
        public async Task UpdateUser_Success()
        {
            var repository = new UserRepository(_context);
            var user = await repository.Add(CreateSampleUser());

            user.Role = "Admin";
            var updated = await repository.Update(user.Username, user);

            Assert.That(updated.Role, Is.EqualTo("Admin"));
        }


        [Test]
        public void UpdateUser_NotFound_ThrowsException()
        {
            var repository = new UserRepository(_context);
            var fakeUser = CreateSampleUser("abc@abc.com");

            var ex = Assert.ThrowsAsync<Exception>(async () => await repository.Update(fakeUser.Username, fakeUser));
            Assert.That(ex.Message, Is.EqualTo("No such item found for updation"));
        }

        [Test]
        public async Task DeleteUser_Succeeds()
        {
            var repository = new UserRepository(_context);
            var user = await repository.Add(CreateSampleUser());

            var deleted = await repository.Delete(user.Username);

            Assert.That(deleted.Username, Is.EqualTo(user.Username));

            var ex = Assert.ThrowsAsync<Exception>(async () => await repository.Get(user.Username));
            Assert.That(ex.Message, Is.EqualTo("No User with the given ID"));
        }

        [Test]
        public void DeleteUser_NotFound_ThrowsException()
        {
            var repository = new UserRepository(_context);

            var ex = Assert.ThrowsAsync<Exception>(async () => await repository.Delete("g@gmail.com"));
            Assert.That(ex.Message, Is.EqualTo("No such item found for deleting"));
        }

        [TearDown]
        public void Cleanup()
        {
            _context.Dispose();
        }
    }
}