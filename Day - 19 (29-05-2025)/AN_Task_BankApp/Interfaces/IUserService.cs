using BankApp.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankApp.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> RegisterUser(CreateUserDto userDto);
        Task<UserDto> GetUserById(int id);
        Task<IEnumerable<UserDto>> GetAllUsers();
    }
}
