using Google.Apis.Auth;
using ocuNotify.Interfaces;
using ocuNotify.Models;
using ocuNotify.Models.DTOs;
using Microsoft.Extensions.Logging;

namespace ocuNotify.Services
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
                _logger.LogError("Google token validation failed: {Message}", ex.Message);
                throw new Exception("Invalid Google token");
            }

            var email = payload.Email;
            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogError("Email from Google token is null or empty.");
                throw new Exception("Invalid email from Google token.");
            }

            var allUsers = await _userRepository.GetAll();
            var user = allUsers.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    Username = payload.Name ?? email, 
                    Role = "HRAdmin", 
                    Password = Guid.NewGuid().ToString()
                };

                await _userRepository.Add(user);
            }

            var token = await _tokenService.GenerateToken(user);

            return new UserLoginResponse
            {
                Username = user.Username,
                Token = token,
            };
        }

    }
}