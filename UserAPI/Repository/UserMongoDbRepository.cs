using MongoDB.Driver;
using Users;

namespace UserAPI.Repository
{
    public class UserMongoDbRepository : IUserRepository
    {
        private const string dbName = "Users";
        private const string collectionName = "User";
        
        private readonly IMongoCollection<User> userCollection;
        public UserMongoDbRepository(IMongoClient client)
        {
            userCollection = client.GetDatabase(dbName).GetCollection<User>(collectionName);

            userCollection.InsertOne(new User
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

        public void CreateUser(User user)
        {
            userCollection.InsertOne(user);
        }

        public void DeleteUser(string login)
        {
            var filter = Builders<User>.Filter.Eq(user => user.Login, login);
            userCollection.DeleteOne(filter);
        }

        public User GetActiveUser(string login)
        {
            return userCollection.Find(user => user.Login == login && user.RevokedBy == null)
                    .FirstOrDefault();
        }

        public User GetAdmin(string login, string password)
        {
            return userCollection.Find(user => user.Login == login && user.Password == password && user.Admin)
                    .FirstOrDefault();
        }

        public User GetUser(string login)
        {
            return userCollection.Find(user => user.Login == login)
                    .FirstOrDefault();
        }

        public IEnumerable<User> GetUsers()
        {
            return userCollection.Find(user => user.RevokedBy == null).ToList();
        }

        public void UpdateUser(User user)
        {
            var filter = Builders<User>.Filter.Eq(replacedUser => replacedUser.Guid, user.Guid);
            userCollection.ReplaceOne(filter,user);
        }
    }
}
