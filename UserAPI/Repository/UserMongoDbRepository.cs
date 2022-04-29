using MongoDB.Driver;
using Users;

namespace UserAPI.Repository
{
    public class UserMongoDbRepository : IUserRepository
    {
        private const string dbName = "Users";
        
        private readonly IMongoCollection<User> userCollection;
        public UserMongoDbRepository(IMongoClient client,string collectionName)
        {
            userCollection = client.GetDatabase(dbName).GetCollection<User>(collectionName);
        }
        public void CreateUser(User user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(string login)
        {
            throw new NotImplementedException();
        }

        public User GetActiveUser(string login)
        {
            throw new NotImplementedException();
        }

        public User GetAdmin(string login, string password)
        {
            throw new NotImplementedException();
        }

        public User GetUser(string login)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetUsers()
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
