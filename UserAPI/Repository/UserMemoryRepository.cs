using Users;

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

    public IEnumerable<User> GetUsers()
    {
        return users.Where(user => user.RevokedBy == null);
    }
    public User GetActiveUser(string login)
    {
        return users.Where(user => user.Login == login && user.RevokedBy == null)
                    .FirstOrDefault();
    }
    public User GetUser(string login)
    {
        return users.Where(user => user.Login == login)
                    .FirstOrDefault();
    }

    public void CreateUser(User user) => users.Add(user);
    public void UpdateUser(User user)
    {
        var index = users.FindIndex(_user => _user.Guid == user.Guid);
        users[index] = user;
    }

    public void DeleteUser(string login)
    {
        var index = users.FindIndex(_user => _user.Login == login);
        users.RemoveAt(index);
    }

    public User GetAdmin(string login, string password)
    {
        return users.Where(user => user.Login == login && user.Password == password && user.Admin)
                    .FirstOrDefault();
    }

}
