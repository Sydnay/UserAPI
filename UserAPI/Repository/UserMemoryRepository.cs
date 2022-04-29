using Users;

public class UserMemoryRepository : IUserRepository
{
    private readonly List<User> users = new()
    {
        //new User()
        //{
        //    Name = "Егор",
        //    Login = "Егорчан",
        //    Password = "Попович",
        //    Gender = 1,
        //    Birthday = new DateTime(1990, 12, 30),
        //    Admin = false,
        //    Guid = Guid.NewGuid(),
        //    CreatedBy = "Putin",
        //    CreatedOn = DateTime.Now,
        //    ModifiedOn = DateTime.Now,
        //    ModifiedBy = "Putin"
        //},
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
        },
        //new User()
        //{
        //    Name = "aboba",
        //    Login = "Abobich",
        //    Password = "NoBitches",
        //    Gender = 1,
        //    Birthday = new DateTime(1991, 2, 3),
        //    Admin = false,
        //    Guid = Guid.NewGuid(),
        //    CreatedBy = "Bidon",
        //    CreatedOn = DateTime.Now,
        //    ModifiedOn = DateTime.Now,
        //    ModifiedBy = "Bidon"
        //}
    };

    public IEnumerable<User> GetUsers()
    {
        return users.Where(user => user.RevokedBy == null);
    }
    public User GetActiveUser(string login)
    {
        return users.Where(user => user.Login == login&&user.RevokedBy==null)
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
        return users.Where(user => user.Login == login&&user.Password == password&&user.Admin)
                    .FirstOrDefault();
    }

}
