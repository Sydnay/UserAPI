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

        public async Task CreateUserAsync(User user)
        {
            await userCollection.InsertOneAsync(user);
        }

        public async Task DeleteUserAsync(string login)
        {
            var filter = Builders<User>.Filter.Eq(user => user.Login, login);
            await userCollection.DeleteOneAsync(filter);
        }

        public async Task<User> GetActiveUserAsync(string login)
        {
            return await userCollection.Find(user => user.Login == login && user.RevokedBy == null)
                    .FirstOrDefaultAsync();
        }

        public async Task<User> GetAdminAsync(string login, string password)
        {
            return await userCollection.Find(user => user.Login == login && user.Password == password && user.Admin)
                    .FirstOrDefaultAsync();
        }

        public async Task<User> GetUserAsync(string login)
        {
            return await userCollection.Find(user => user.Login == login)
                    .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await userCollection.Find(user => user.RevokedBy == null).ToListAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            var filter = Builders<User>.Filter.Eq(replacedUser => replacedUser.Guid, user.Guid);
            await userCollection.ReplaceOneAsync(filter,user);
        }
    }
}
