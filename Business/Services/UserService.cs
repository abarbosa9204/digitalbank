using Data.Repositories;
using Grpc.Core;

namespace Business.Services
{
    public class UserService : Users.UsersBase
    {
        private readonly IUsersRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration _configuration;

        public UserService(IUsersRepository userRepository, ILogger<UserService> logger, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _logger = logger;
            _configuration = configuration;
        }

        public override async Task<UserResponse> CreateUser(UserRequest request, ServerCallContext context)
        {
            {
                _logger.LogInformation("Creando un nuevo usuario.");

                try
                {
                    DateTime fechaNacimiento;

                    if (!DateTime.TryParse(request.FechaNacimiento, out fechaNacimiento))
                    {
                        _logger.LogError("La fecha proporcionada no es válida.");
                        throw new RpcException(new Status(StatusCode.InvalidArgument, "La fecha proporcionada no es válida."));
                    }

                    var user = new Data.Models.Users
                    {
                        Uuid = Guid.NewGuid(),
                        Nombre = request.Nombre,
                        FechaNacimiento = fechaNacimiento,
                        Sexo = request.Sexo,
                        Estado = true
                    };

                    await _userRepository.CreateUserAsync(user);

                    return new UserResponse
                    {
                        Uuid = user.Uuid.ToString(),
                        Estado = user.Estado
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al crear un nuevo usuario.");
                    throw new RpcException(new Status(StatusCode.Internal, "Error al crear un nuevo usuario."), ex.Message);
                }
            }
        }
        public override async Task<UserResponse> UpdateUser(UserRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Actualizando el usuario usuario.");

            try
            {
                DateTime fechaNacimiento;

                if (!DateTime.TryParse(request.FechaNacimiento, out fechaNacimiento))
                {
                    _logger.LogError("La fecha proporcionada no es válida.");
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "La fecha proporcionada no es válida."));
                }

                if (!Guid.TryParse(request.Uuid, out Guid uuid))
                {
                    _logger.LogError("El UUID proporcionado no es válido.");
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "El UUID proporcionado no es válido."));
                }

                var user = new Data.Models.Users
                {
                    Uuid = uuid,
                    Nombre = request.Nombre,
                    FechaNacimiento = fechaNacimiento,
                    Sexo = request.Sexo,
                    Estado = true
                };

                await _userRepository.UpdateUserAsync(user);

                return new UserResponse
                {
                    Uuid = user.Uuid.ToString(),
                    Estado = user.Estado
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el usuario.");
                throw new RpcException(new Status(StatusCode.Internal, "Error al actualizar el usuario."), ex.Message);
            }
        }


        public override async Task<UserResponse> DeleteUser(UserRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Eliminando el usuario usuario.");

            try
            {
                if (!Guid.TryParse(request.Uuid, out Guid uuid))
                {
                    _logger.LogError("El UUID proporcionado no es válido.");
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "El UUID proporcionado no es válido."));
                }

                var user = new Data.Models.Users
                {
                    Uuid = uuid
                };

                await _userRepository.DeleteUserAsync(user);

                return new UserResponse
                {
                    Uuid = user.Uuid.ToString(),
                    Estado = user.Estado
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el usuario.");
                throw new RpcException(new Status(StatusCode.Internal, "Error al eliminar el usuario."), ex.Message);
            }
        }
        public async Task<(List<Data.Models.Users> Users, int TotalRecords)> GetUsersPagedAsync(int pageNumber, int pageSize)
        {
            return await _userRepository.GetUsersPagedAsync(pageNumber, pageSize);
        }

        public async Task<Data.Models.Users> GetUserByUuid(Guid uuid)
        {
            var user = await _userRepository.GetUserByUuidAsync(uuid);
            return user;
        }
    }
}
