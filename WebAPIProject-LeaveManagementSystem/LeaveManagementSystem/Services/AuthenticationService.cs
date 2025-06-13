using LeaveManagementSystem.Interfaces;
using LeaveManagementSystem.Models;
using LeaveManagementSystem.Models.DTOs;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;

namespace LeaveManagementSystem.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRepository<Guid, User> _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthenticationService> _logger;

        private static readonly ConcurrentDictionary<string, string> _refreshTokens = new();

        public AuthenticationService(
            IRepository<Guid, User> userRepository,
            ITokenService tokenService,
            IMapper mapper,
            ILogger<AuthenticationService> logger)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserLoginResponse> LoginAsync(UserLoginRequest request)
        {
            var user = (await _userRepository.GetAll()).FirstOrDefault(u =>
                u.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                _logger.LogWarning("Failed login attempt for {Email}", request.Email);
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            _refreshTokens[user.Email] = refreshToken;

            _logger.LogInformation("User {Email} logged in successfully", user.Email);

            return new UserLoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                User = _mapper.Map<UserDto>(user)
            };
        }

        public async Task<string> RefreshTokenAsync(string refreshToken)
        {
            var email = _refreshTokens.FirstOrDefault(x => x.Value == refreshToken).Key;

            if (email == null)
            {
                _logger.LogWarning("Invalid refresh token attempt");
                throw new SecurityTokenException("Invalid refresh token");
            }

            if (_refreshTokens[email] != refreshToken)
            {
                _logger.LogWarning("Refresh token mismatch for {Email}", email);
                throw new SecurityTokenException("Refresh token mismatch");
            }

            var user = (await _userRepository.GetAll()).FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                _logger.LogWarning("User not found for refresh token with email {Email}", email);
                throw new UnauthorizedAccessException("User not found");
            }

            _logger.LogInformation("Refresh token used to generate new access token for {Email}", email);

            return _tokenService.GenerateAccessToken(user);
        }

        public Task LogoutAsync(string email)
        {
            var removed = _refreshTokens.TryRemove(email, out _);
            if (removed)
            {
                _logger.LogInformation("User {Email} logged out", email);
            }
            else
            {
                _logger.LogWarning("Logout attempted for unknown user {Email}", email);
            }

            return Task.CompletedTask;
        }

        public async Task<UserDto> GetCurrentUserAsync(ClaimsPrincipal userPrincipal)
        {
            var email = userPrincipal.FindFirstValue(ClaimTypes.Email);
            var user = (await _userRepository.GetAll()).FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                _logger.LogWarning("GetCurrentUser failed: User not found with email {Email}", email);
                throw new UnauthorizedAccessException("User not found");
            }

            return _mapper.Map<UserDto>(user);
        }
    }
}
