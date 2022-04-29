using Users;

public interface IUserRepository
{
    User GetActiveUser(string login);
    User GetUser(string login);
    User GetAdmin(string login, string password);
    IEnumerable<User> GetUsers();
    void CreateUser(User user);
    void UpdateUser(User user);
    void DeleteUser(string login);
}