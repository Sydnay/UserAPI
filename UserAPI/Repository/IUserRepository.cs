using Users;

namespace UserAPI.Repository
{
    public interface IUserRepository
    {
        Task<User> GetActiveUserAsync(string login);
        Task<User> GetUserAsync(string login);
        Task<User> GetAdminAsync(string login, string password);
        Task<IEnumerable<User>> GetUsersAsync();
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(string login);
    }
}