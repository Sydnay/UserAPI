using Microsoft.EntityFrameworkCore;
using Users;

namespace UserAPI.Repository
{
    public class UserEFRepository : DbContext, IUserRepository
    {
        public UserEFRepository(DbContextOptions<UserEFRepository> options) : base(options) 
        {
            if (!Users.Any())
            {
                Users.Add(new User
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
                });
            }
            SaveChanges();
        }
        public DbSet<User> Users { get; set; }


        public async Task CreateUserAsync(User user)
        {
            await Users.AddAsync(user);
            SaveChanges();
        }

        public async Task DeleteUserAsync(string login)
        {
            var user = await Users.FirstOrDefaultAsync(x => x.Login == login);
            Users.Remove(user);
            SaveChanges();
        }

        public async Task<User> GetActiveUserAsync(string login)
        {
            return await Users.Where(user => user.Login == login && user.RevokedBy == null)
                    .FirstOrDefaultAsync();
        }

        public async Task<User> GetAdminAsync(string login, string password)
        {
            return await Users.Where(user => user.Login == login && user.Password == password
                                                    && user.Admin
                                                    && user.RevokedBy == null).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserAsync(string login)
        {
            return await Users.Where(user => user.Login == login)
                    .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await Users.ToListAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            User updatedUser = await Users.FindAsync(user.Guid);
            //Здесь явно можно было сделать красивее и правильнее
            updatedUser.Name = user.Name;
            updatedUser.Login = user.Login;
            updatedUser.Password = user.Password;
            updatedUser.Gender = user.Gender;
            updatedUser.Admin = user.Admin;
            updatedUser.ModifiedOn = user.ModifiedOn;
            updatedUser.ModifiedBy = user.ModifiedBy;
            updatedUser.RevokedBy = user.RevokedBy;
            updatedUser.RevokedOn = user.RevokedOn;

            Users.Update(updatedUser);
            SaveChanges();
        }
    }
}
