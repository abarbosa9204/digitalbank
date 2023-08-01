using Data.Repositories;
using Grpc.Core;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Business.Services
{
    public class ApplicationUserService : applicationUsers.applicationUsersBase
    {
        private readonly IUsersRepository _userRepository;
        private readonly ILogger<ApplicationUserService> _logger;
        private readonly IConfiguration _configuration;

        public ApplicationUserService(IUsersRepository userRepository, ILogger<ApplicationUserService> logger, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _logger = logger;
            _configuration = configuration;
        }

        public override async Task<RegisterResponse> RegisterUser(RegisterRequest registerRequest, ServerCallContext context)
        {
            _logger.LogInformation("Registrando un nuevo usuario.");

            try
            {
                var existingUser = await _userRepository.GetUserByUserEmailAsync(registerRequest.Email);

                if (existingUser != null)
                {
                    _logger.LogError("El nombre de usuario ya está en uso.");
                    throw new RpcException(new Status(StatusCode.AlreadyExists, "El nombre de usuario ya está en uso."));
                }

                var applicationUser = new Data.Models.ApplicationUser
                {
                    Uuid = Guid.NewGuid(),
                    Nombre = registerRequest.Username,
                    Email = registerRequest.Email,
                    PasswordHash = HashPassword(registerRequest.Password),
                    Telefono = registerRequest.PhoneNumber,
                };

                Guid? uuid = await _userRepository.RegisterUserAsync(applicationUser);

                if (uuid.HasValue)
                {                    
                    var token = GenerateJwtToken(applicationUser, _configuration); ;
                    return new RegisterResponse
                    {
                        Uuid = applicationUser.Uuid.ToString(),
                        Estado = true,
                        Token = token 
                    };
                }
                else
                {
                    _logger.LogError("El token no fue generado");
                    throw new RpcException(new Status(StatusCode.AlreadyExists, "El token no fue generado."));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar un nuevo usuario.");
                throw new RpcException(new Status(StatusCode.Internal, "Error al registrar un nuevo usuario."), ex.Message);
            }
        }


        public async Task<string> AuthenticateUserAsync(string username, string password)
        {
            try
            {
                var applicationUser = await _userRepository.GetUserByUserEmailAsync(username);
                if (applicationUser != null && VerifyPassword(password, applicationUser.PasswordHash))
                {
                    return GenerateJwtToken(applicationUser, _configuration);
                }
                return null;
                _logger.LogError("Las credenciales proporcionadas no son correctas.");
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Las credenciales proporcionadas no son correctas."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al autenticar al usuario.");
                throw;
            }
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            if (hashedPassword == null)
            {
                return false;
            }
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }


        private string GenerateJwtToken(Data.Models.ApplicationUser applicationUser, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JWTSettings");
            string key = jwtSettings["Key"];
            string issuer = jwtSettings["Issuer"];
            string audience = jwtSettings["Audience"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Email),
                new Claim("uuid", applicationUser.Uuid.ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(jwtSettings.GetValue<int>("DurationInMinutes", 30)),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
