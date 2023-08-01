using Data.Models;

namespace Data.Repositories
{
    public interface IUsersRepository
    {
        Task<Users> CreateUserAsync(Users user);
        Task<Users> GetUserByUuidAsync(Guid value);
        Task<(List<Users> Users, int TotalRecords)> GetUsersPagedAsync(int pageNumber, int pageSize);
        Task<bool> UpdateUserAsync(Users user);
        Task<bool> DeleteUserAsync(Users user);
        Task<ApplicationUser> GetUserByUserEmailAsync(string username);
        Task<Guid?> RegisterUserAsync(ApplicationUser applicationUser);
    }
}
