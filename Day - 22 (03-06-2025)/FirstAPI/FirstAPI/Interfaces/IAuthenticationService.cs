using FirstAPI.Models.DTOs;

namespace FirstAPI.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<UserLoginResponse> Login(UserLoginRequest user);
    }
}