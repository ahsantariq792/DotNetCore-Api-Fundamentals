using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CRUD_Operation.Entities;
using CRUD_Operation.Models.Auth;
using CRUD_Operation.Repositories.Interfaces;
using CRUD_Operation.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CRUD_Operation.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<AuthResponse?> SignupAsync(SignupRequest request, CancellationToken cancellationToken = default)
        {
            var existing = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (existing != null)
                return null;

            var user = new User
            {
                Email = request.Email.Trim().ToLowerInvariant(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                FullName = request.FullName?.Trim()
            };

            await _userRepository.AddAsync(user, cancellationToken);
            return BuildAuthResponse(user);
        }

        public async Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email.Trim().ToLowerInvariant(), cancellationToken);
            if (user == null)
                return null;

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return null;

            return BuildAuthResponse(user);
        }

        private AuthResponse BuildAuthResponse(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiresMinutes"] ?? "60"));

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResponse
            {
                Token = tokenString,
                Email = user.Email,
                FullName = user.FullName,
                ExpiresAt = expires
            };
        }
    }
}
