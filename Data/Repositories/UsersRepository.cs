using Data.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Data.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AppDbContext _context;

        public UsersRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Users> CreateUserAsync(Users user)
        {
            var idParameter = new SqlParameter
            {
                ParameterName = "@Id",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };

            var parameters = new[]
            {
                new SqlParameter("@Accion", "C"),
                new SqlParameter("@Uuid", user.Uuid),
                new SqlParameter("@Nombre", user.Nombre),
                new SqlParameter("@FechaNacimiento", user.FechaNacimiento),
                new SqlParameter("@Sexo", user.Sexo),
                new SqlParameter("@Estado", 1),
                idParameter
            };

            await _context.Database.ExecuteSqlRawAsync("EXECUTE sp_users @Accion, @Id OUTPUT, @Uuid, @Nombre, @FechaNacimiento, @Sexo, @Estado", parameters);
            int id = Convert.ToInt32(idParameter.Value);
            
            var createdUser = await _context.Users.FindAsync(id);
            
            if (createdUser == null)
            {
                throw new InvalidOperationException("No se encontró el usuario creado en la base de datos.");
            }

            return createdUser;
        }
        public async Task<(List<Users> Users, int TotalRecords)> GetUsersPagedAsync(int pageNumber, int pageSize)
        {
            var parameters = new[]
            {
                new SqlParameter("@Accion", "R"),
                new SqlParameter("@Id", DBNull.Value),
                new SqlParameter("@Uuid", DBNull.Value),
                new SqlParameter("@Nombre", DBNull.Value),
                new SqlParameter("@FechaNacimiento", DBNull.Value),
                new SqlParameter("@Sexo", DBNull.Value),
                new SqlParameter("@Estado", DBNull.Value),
                new SqlParameter("@PageNumber", pageNumber),
                new SqlParameter("@PageSize", pageSize),
                new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output }
            };
            
            #pragma warning disable CS8603
            var users = await _context.Users.FromSqlRaw("EXECUTE sp_users @Accion, @Id, @Uuid, @Nombre, @FechaNacimiento, @Sexo, @Estado, @PageNumber, @PageSize, @TotalRecords OUTPUT", parameters)
                                            .AsNoTracking()
                                            .ToListAsync();

            var totalRecords = (int)parameters.FirstOrDefault(p => p.ParameterName == "@TotalRecords")?.Value;

            return (users, totalRecords);
        }

        public async Task<Users> GetUserByUuidAsync(Guid uuid)
        {
            var parameters = new[]
            {
                new SqlParameter("@Accion", "R"),
                new SqlParameter("@Id", DBNull.Value),
                new SqlParameter("@Uuid", uuid),
                new SqlParameter("@Nombre", DBNull.Value),
                new SqlParameter("@FechaNacimiento", DBNull.Value),
                new SqlParameter("@Sexo", DBNull.Value),
                new SqlParameter("@Estado", DBNull.Value),
                new SqlParameter("@PageNumber", DBNull.Value),
                new SqlParameter("@PageSize", DBNull.Value)
            };

            var users = await _context.Users.FromSqlRaw("EXECUTE sp_users @Accion, @Id, @Uuid, @Nombre, @FechaNacimiento, @Sexo, @Estado, @PageNumber, @PageSize", parameters)
                                            .AsNoTracking()
                                            .ToListAsync();

            return users.FirstOrDefault();
        }
        public async Task<bool> UpdateUserAsync(Users user)
        {
            var parameters = new[]
                {
                new SqlParameter("@Accion", "U"),
                new SqlParameter("@Id", DBNull.Value),
                new SqlParameter("@Uuid", user.Uuid),
                new SqlParameter("@Nombre", user.Nombre),
                new SqlParameter("@FechaNacimiento", user.FechaNacimiento),
                new SqlParameter("@Sexo", user.Sexo),
                new SqlParameter("@Estado", DBNull.Value),
                new SqlParameter("@PageNumber", DBNull.Value),
                new SqlParameter("@PageSize", DBNull.Value)
            };

            await _context.Database.ExecuteSqlRawAsync("EXECUTE sp_users @Accion, @Id, @Uuid, @Nombre, @FechaNacimiento, @Sexo, @Estado, @PageNumber, @PageSize", parameters);
            int rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0;
        }
        public async Task<bool> DeleteUserAsync(Users user)
        {
            var parameters = new[]
                {
                new SqlParameter("@Accion", "D"),
                new SqlParameter("@Id", DBNull.Value),
                new SqlParameter("@Uuid", user.Uuid),
                new SqlParameter("@Nombre", DBNull.Value),
                new SqlParameter("@FechaNacimiento", DBNull.Value),
                new SqlParameter("@Sexo", DBNull.Value),
                new SqlParameter("@Estado", DBNull.Value),
                new SqlParameter("@PageNumber", DBNull.Value),
                new SqlParameter("@PageSize", DBNull.Value)
            };

            await _context.Database.ExecuteSqlRawAsync("EXECUTE sp_users @Accion, @Id, @Uuid, @Nombre, @FechaNacimiento, @Sexo, @Estado, @PageNumber, @PageSize", parameters);
            int rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0;
        }
        public async Task<ApplicationUser> GetUserByUserEmailAsync(string email)
        {
            var parameters = new[]
            {
                new SqlParameter("@Accion", "R"),                
                new SqlParameter("@Uuid", DBNull.Value),
                new SqlParameter("@Nombre", DBNull.Value),
                new SqlParameter("@Email", email),
            };
            #pragma warning disable CS8603
            var applicationUser = await _context.ApplicationUsers.FromSqlRaw("EXECUTE sp_application_user @Accion, @Uuid, @Nombre, @Email", parameters)
                                            .AsNoTracking()
                                            .ToListAsync();

            return applicationUser.FirstOrDefault();
        }

        public async Task<Guid?> RegisterUserAsync(ApplicationUser applicationUser)
        {
            var uuidParameter = new SqlParameter
            {
                ParameterName = "@Uuid",
                SqlDbType = SqlDbType.UniqueIdentifier,
                Direction = ParameterDirection.Output
            };

            var registroCreadoParameter = new SqlParameter
            {
                ParameterName = "@RegistroCreado",
                SqlDbType = SqlDbType.Bit,
                Direction = ParameterDirection.Output
            };

            var parameters = new[]
                {
                    new SqlParameter("@Accion", "C"),
                    new SqlParameter("@Uuid", applicationUser.Uuid),
                    new SqlParameter("@Nombre", applicationUser.Nombre),
                    new SqlParameter("@Email", applicationUser.Email),
                    new SqlParameter("@PasswordHash", applicationUser.PasswordHash),
                    new SqlParameter("@Telefono", applicationUser.Telefono),
                    new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output },
                    new SqlParameter("@RegistroCreado", SqlDbType.Bit) { Direction = ParameterDirection.Output },
                    new SqlParameter("@RegistroActualizado", SqlDbType.Bit) { Direction = ParameterDirection.Output },
                    new SqlParameter("@RegistroEliminado", SqlDbType.Bit) { Direction = ParameterDirection.Output }
                };

            await _context.Database.ExecuteSqlRawAsync("EXECUTE sp_application_user @Accion, @Uuid OUTPUT, @Nombre, @Email, @PasswordHash, @Telefono, @TotalRecords OUTPUT, @RegistroCreado OUTPUT, @RegistroActualizado OUTPUT, @RegistroEliminado OUTPUT", parameters);

            bool registroCreado = Convert.ToBoolean(parameters.Single(p => p.ParameterName == "@RegistroCreado").Value);            

            if (registroCreado)
            {
                return applicationUser.Uuid;
            }
            else
            {
                return null;
            }
        }
    }
}
