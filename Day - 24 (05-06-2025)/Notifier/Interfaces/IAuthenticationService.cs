using ocuNotify.Models.DTOs;

namespace ocuNotify.Interfaces
{
    public interface IAuthenticationService
    {
        Task<UserLoginResponse> LoginWithGoogle(string idToken);
    }
}