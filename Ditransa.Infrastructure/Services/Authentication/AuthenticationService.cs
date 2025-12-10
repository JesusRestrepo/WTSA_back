using Ditransa.Application.DTOs.Authentication;
using Ditransa.Application.DTOs.Menu;
using Ditransa.Application.DTOs.Users;
using Ditransa.Application.Interfaces.Authentication;
using Ditransa.Application.Interfaces.Repositories.Menu;
using Ditransa.Application.Interfaces.Repositories.MenuRol;
using Ditransa.Application.Interfaces.Repositories.Users;
using Ditransa.Domain.Entities.WTSA;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Ditransa.Infrastructure.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMenuRepository _menuRepository;
        private readonly IMenuRolRepository _menuRolRepository;
        public AuthenticationService(IUserRepository userRepository, ILogger<AuthenticationService> logger, IConfiguration configuration, IMenuRepository menuRepository, IMenuRolRepository menuRolRepository)
        {
            _userRepository = userRepository;
            _logger = logger;
            _configuration = configuration;
            _menuRepository = menuRepository;
            _menuRolRepository = menuRolRepository;
        }
        public async Task<User> Register(CreateUserDto user, string password, bool canLogin)
        {
            try
            {
                User? userExists = await _userRepository.FindByEmailAsync(user.Email);
                if (userExists != null)
                {
                    _logger.LogWarning("Attempt to register an already existing user with email: {Email}", user.Email);
                    throw new InvalidOperationException("User already exists");
                }

                string salt = GenerateSalt();
                string hashedPassword = GenerateHashedPassword(password, salt);

                var newUser = new User
                {
                    UserId = Guid.NewGuid(),
                    Email = user.Email,
                    Salt = salt,
                    HashedPwd = hashedPassword,
                    CreatedAt = DateTime.UtcNow,
                    CanLogin = canLogin,
                    RoleId = user.RolId,
                    Position = user.Position
                };
                var createdUser = await _userRepository.AddAsync(newUser);
                await _userRepository.Save();

                _logger.LogInformation("User registered successfully with email: {Email}", user.Email);
                return createdUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering user with email: {Email}", user.Email);
                throw;
            }

        }

        /// <summary>
        /// Generates a cryptographic salt
        /// </summary>
        /// <returns></returns>
        public static string GenerateSalt()
        {
            var salt = RandomNumberGenerator.GetBytes(16);
            return Convert.ToBase64String(salt);
        }

        /// <summary>
        /// Generates a hashed password using PBKDF2
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string GenerateHashedPassword(string password, string salt)
        {
            var saltByteArray = Convert.FromBase64String(salt);

            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltByteArray,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashedPassword;
        }

        public async Task<LoginResultDto> Login(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.FindByEmailAsync(email);

            if (user == null)
            {
                _logger.LogWarning("Login attempt failed for non-existing user with email: {Email}", email);
                return new LoginResultDto
                {
                    UserFound = false,
                    ValidCredentials = false,
                    Success = false,
                };
            }

            bool validCredentials = ValidateCredentials(user, password);
            if (!validCredentials)
            {
                return new LoginResultDto
                {
                    UserFound = true,
                    ValidCredentials = false,
                    Success = false
                };
            }

            var token = await GenerateJwtToken(user);

            var menuRoles = await _menuRolRepository
                .Entities
                .Where(mr => mr.RoleId == user.RoleId)
                .ToListAsync(cancellationToken);

            var menuIds = menuRoles
                .Select(mr => mr.MenuId)
                .Distinct()
                .ToList();

            var menus = await _menuRepository
                .Entities
                .Where(m => menuIds.Contains(m.MenuId) && m.Active)
                .Select(m => new MenuDto
                {
                    MenuId = m.MenuId,
                    Type = m.Type,
                    Value = m.Value,
                    Active = m.Active,
                    Link = m.Link,
                    Parent = m.Parent
                })
                .ToListAsync(cancellationToken);

            _logger.LogInformation("User logged in successfully with email: {Email}", email);

            var userDto = new UserDto
            {
                UserId = user.UserId,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                CanLogin = user.CanLogin,
                RoleId = user.RoleId,
                Position = user.Position,
                RoleDescription = user.Role?.Description
            };

            return new LoginResultDto
            {
                Email = user.Email,
                ValidCredentials = true,
                UserFound = true,
                Success = true,
                User = userDto,
                Token = token,
                Menus = menus
            };
        }


        private bool ValidateCredentials(User? user, string password) => user != null && VerifyPassword(user, password);

        private static bool VerifyPassword(User user, string password) =>
            user.HashedPwd.Equals(GenerateHashedPassword(password, user.Salt)) && user.CanLogin;

        public async Task<string> GenerateJwtToken(User user, int hours = 1, CancellationToken cancellationToken = default)
        {

            // Obtener la clave secreta y preparar los objetos de seguridad
            var secretKey = _configuration["JwtBearer:SecretKey"];
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            var newUser = await _userRepository.FindByEmailAsync(user.Email);
            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (JwtRegisteredClaimNames.NameId, newUser!.UserId.ToString()),
                new (ClaimTypes.Name, newUser.Email),
                new (ClaimTypes.Role, newUser.RoleId.ToString()),
                new (ClaimTypes.NameIdentifier, newUser.Role.Description)

            };

            var payload = new JwtPayload(
                issuer: _configuration["JwtBearer:Issuer"],
                audience: _configuration["JwtBearer:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(hours));

            var token = new JwtSecurityToken(header, payload);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
