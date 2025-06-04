using Google.Apis.Auth;
using FirstAPI.Interfaces;
using FirstAPI.Models;
using FirstAPI.Models.DTOs;
using Microsoft.Extensions.Logging;

namespace FirstAPI.Services
{
    public class GoogleAuthenticationService : IAuthenticationService
    {
        private readonly ITokenService _tokenService;
        private readonly IRepository<string, User> _userRepository;
        private readonly ILogger<GoogleAuthenticationService> _logger;

        public GoogleAuthenticationService(ITokenService tokenService,
                                    IRepository<string, User> userRepository,
                                    ILogger<GoogleAuthenticationService> logger)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<UserLoginResponse> LoginWithGoogle(string idToken)
        {
            GoogleJsonWebSignature.Payload payload;

            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Google token validation failed", ex.Message);
                throw new Exception("Invalid Google token");
            }

            var email = payload.Email;

            var dbUser = await _userRepository.Get(email);
            if (dbUser == null)
            {
                dbUser = new User
                {
                    Username = email,
                    Role = "Doctor", 
                };
                await _userRepository.Add(dbUser);
            }

            var token = await _tokenService.GenerateToken(dbUser);

            return new UserLoginResponse
            {
                Username = dbUser.Username,
                Token = token,
            };
        }
    }
}