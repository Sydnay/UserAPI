using Users;

namespace UserAPI.Repository
{
    public class UserMemoryRepository : IUserRepository
    {
        private readonly List<User> users = new()
        {
            new User()
            {
                Name = "Admin",
                Login = "admin",
                Password = "admin",
                Gender = 2,
                Birthday = new DateTime(1945, 6, 22),
                Admin = true,
                Guid = Guid.NewGuid(),
                CreatedBy = "Server",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedBy = "Server"
            }
        };

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var list = users.Where(user => user.RevokedBy == null);
            return await Task.FromResult(list);
        }
        public async Task<User> GetActiveUserAsync(string login)
        {
            var user = users.Where(user => user.Login == login && user.RevokedBy == null)
                        .FirstOrDefault();
            return await Task.FromResult(user);
        }
        public async Task<User> GetUserAsync(string login)
        {
            var user = users.Where(user => user.Login == login)
                        .FirstOrDefault();
            return await Task.FromResult(user);
        }

        public async Task CreateUserAsync(User user)
        {
            users.Add(user);
            await Task.CompletedTask;
        }

        public async Task UpdateUserAsync(User user)
        {
            var index = users.FindIndex(_user => _user.Guid == user.Guid);
            users[index] = user;
            await Task.CompletedTask;
        }

        public async Task DeleteUserAsync(string login)
        {
            var index = users.FindIndex(_user => _user.Login == login);
            users.RemoveAt(index);
            await Task.CompletedTask;
        }

        public async Task<User> GetAdminAsync(string login, string password)
        {
            var admin = users.Where(user => user.Login == login && user.Password == password && user.Admin)
                        .FirstOrDefault();
            return await Task.FromResult(admin);
        }

    }
}