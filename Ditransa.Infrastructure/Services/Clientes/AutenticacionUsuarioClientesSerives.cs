using AutoMapper;
using Ditransa.Application.DTOs;
using Ditransa.Application.DTOs.Clientes;
using Ditransa.Application.Interfaces.Clientes;
using Ditransa.Application.Interfaces.Repositories;
using Ditransa.Application.Interfaces.Repositories.Clientes;
using Ditransa.Domain.Entities;
using Ditransa.Domain.Entities.Clientes;
using Ditransa.Shared;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ditransa.Infrastructure.Services.Clientes
{
    public class AutenticacionUsuarioClientesSerives : IautenticacionUsuarioClientesService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsuarioClienteRepository _usuarioClienteRepository;
        private readonly IAccesoClientesRepository _accesoClientesRepository;
        private static readonly Dictionary<string, string> _refreshTokens = new();

        public AutenticacionUsuarioClientesSerives(IConfiguration configuration, IMapper mapper, IUnitOfWork unitOfWork, IUsuarioClienteRepository usuarioClienteRepository, IAccesoClientesRepository accesoClientesRepository)
        {
            _configuration = configuration;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _usuarioClienteRepository = usuarioClienteRepository;
            _accesoClientesRepository = accesoClientesRepository;
        }

        public string GenerateJwtToken(UsuarioClientes user)
        {
            var secretKey = _configuration.GetSection("JwtBearer:SecretKey").Value;
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);
            var usuarioAcceso = _unitOfWork.Repository<UsuarioClientes>().Entities.Where(c => c.Email == user.Email).FirstOrDefault();

            var claims = new List<Claim>
            {
                new Claim("correo", user.Email),
                new Claim("nombre", user.Nombre),
                new Claim("rol", user.RolId)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtBearer:Issuer"],
                audience: _configuration["JwtBearer:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(1),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public async Task<ResultLoginClientesDto> Login(string email, string password)
        {

            DateTime accesoFecha = DateTime.Now;
            int horaHora = accesoFecha.Hour;
            short horaShort = (short)horaHora;
            string horaAcceso = accesoFecha.ToString("HH:mm:ss");
            int mesAcceso = accesoFecha.Month;
            short mesShort = (short)mesAcceso;

            var user = await _usuarioClienteRepository.FindByEmailAsync(email);
            var token = string.Empty;
            var refreshToken = string.Empty;
            if (user == null)
            {
                return new ResultLoginClientesDto
                {
                    ErrorMessage = "Usuario no existe"
                };
            }
            else
            {
                if(user.Estado == "A")
                {
                    if (!VerifyPassword(user, password))
                    {
                        return new ResultLoginClientesDto
                        {
                            ErrorMessage = "Contraseña incorrecta"
                        };
                    }
                }
                else
                {
                    return new ResultLoginClientesDto
                    {
                        ErrorMessage = "Usuario inactivo"
                    };
                }
            }
            

            var registroAcceso = new AccesoClientes
            {
                AccesoFecha = accesoFecha,
                HoraHora = horaShort,
                AccesoHoras = horaAcceso,
                Email = user.Email,
                AccesoMes = mesShort,
                AccesoTipoUsuario = user.Tipo
            };

            var accesoCreated = await _accesoClientesRepository.AddAsync(registroAcceso);
            await _usuarioClienteRepository.Save();

            token = GenerateJwtToken(user);
            refreshToken = GenerateRefreshToken();


        return new ResultLoginClientesDto { token = token, refreshToken = refreshToken };
        }

        public async Task<UsuarioClientes> Register(UsuarioClientes clientes)
        {
            var userExists = await _usuarioClienteRepository.FindByEmailAsync(clientes.Email);
            if (userExists != null)
            {
                throw new Exception("Ya existe un usuario con ese email.");
            }

            var newUser = new UsuarioClientes
            {
                Email = clientes.Email,
                Nombre = clientes.Nombre,
                Documento = clientes.Documento,
                FechaNacimiento = clientes.FechaNacimiento,
                Celular = clientes.Celular,
                Address = clientes.Address,
                Cargo = clientes.Cargo,
                Area = clientes.Area,
                KeyUser = clientes.KeyUser,
                Pwd = clientes.Pwd,
                Estado = clientes.Estado,
                EmpresaNit = clientes.EmpresaNit,
                Tipo = clientes.Tipo,
                RolId = clientes.RolId,
                TipoDocumento = clientes.TipoDocumento
            };

            var createdUser = await _usuarioClienteRepository.AddAsync(newUser);
            await _usuarioClienteRepository.Save();
            return createdUser;
        }

        private static bool VerifyPassword(UsuarioClientes user, string password)
        {
            return user.Pwd.Equals(GenerateHashedPassword(password, user.KeyUser));
        }

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

        public async Task<RefreshToken> RefreshToken(string token, string refreshToken)
        {
            var principal = GetPrincipalFromExpiredToken(token);
            var userEmail = principal.Claims.First(c => c.Type == "correo").Value;

            var user = await _usuarioClienteRepository.FindByEmailAsync(userEmail);
            //if (user == null || !IsRefreshTokenValid(userEmail, refreshToken))
            //    throw new SecurityTokenException("Token inválido");

            var newToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            _refreshTokens[userEmail] = newRefreshToken;

            return new RefreshToken
            {
                token = newToken,
                refreshToken = newRefreshToken,
            };
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtBearer:SecretKey"])),
                ValidIssuer = _configuration["JwtBearer:Issuer"],
                ValidAudience = _configuration["JwtBearer:Audience"]
            };

            return tokenHandler.ValidateToken(token, validationParameters, out _);
        }

        private bool IsRefreshTokenValid(string userEmail, string refreshToken)
        {
            var respuesta = _refreshTokens.TryGetValue(userEmail, out var storedToken) && storedToken == refreshToken;
            return respuesta;
        }
    }
}
