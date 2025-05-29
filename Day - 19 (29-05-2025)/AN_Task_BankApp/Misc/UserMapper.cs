using BankApp.DTOs;
using BankApp.Models;

namespace BankApp.Mappers
{
    public class UserMapper
    {
        public User? MapCreateUserDtoToUser(CreateUserDto dto)
        {
            return new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };
        }

        public UserDto? MapUserToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        public IEnumerable<UserDto> MapUserListToUserDtoList(IEnumerable<User> users)
        {
            return users.Select(MapUserToUserDto)!;
        }

    }
}
