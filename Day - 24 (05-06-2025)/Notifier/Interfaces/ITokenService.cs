using ocuNotify.Models;

namespace ocuNotify.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(User user);
    }
}