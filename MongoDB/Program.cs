using MongoDB.Bson;
using MongoDB.Driver;
using Users;

namespace Mongo
{
    class Program
    {
        static void Main(string[] args)
        {
            var user = new User()
            {
                Name = "aboba",
                Login = "Abobich",
                Password = "NoBitches",
                Gender = 1,
                Birthday = new DateTime(1991, 2, 3),
                Admin = false,
                Guid = Guid.NewGuid(),
                CreatedBy = "Putin",
                ModifiedBy = "Putin"
            };

            var db = new MongoRepository("Users");
            db.InsertUser("User", user);
            var user1 = db.GetUser("User", "Egorchan@aboba.ru");
            var users = db.GetAllActiveUsers("User");
            foreach (var useraboba in users) { Console.WriteLine($"{useraboba.ToString()}"); }
            var usersOlderThan = db.GetUsersOlderThan("User", 30);
            foreach (var useraboba in usersOlderThan)
            { Console.WriteLine($"{useraboba.ToString()}"); }
            //db.DeleteUser("User", "OkaaayLesssgo", true);
            //db.RevokeUser("User", "OkaaayLesssgo");
        }
    }

    public class MongoRepository
    {
        private IMongoDatabase db;
        public MongoRepository(string name)
        {
            var client = new MongoClient();
            db = client.GetDatabase(name);
        }
        /// <summary>
        /// 1) Создание пользователя по логину, паролю, имени, полу и дате рождения + указание будет ли 
        /// пользователь админом (Доступно Админам)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <param name="user"></param>
        public void InsertUser(string table, User user)// Вроде ок
        {
            var collection = db.GetCollection<User>(table);
            foreach(var symb in user.Login) 
            {
                if (!Char.IsLetterOrDigit(symb))
                { 
                    Console.WriteLine("Invalid login"); 
                    return; 
                }   
            };
            bool isExist = collection.Find(_ => _.Login == user.Login).Any();
            if (isExist) 
            { 
                Console.WriteLine("This user is already exist"); 
                return;
            }
            collection.InsertOne(user);

        }
        public void UpdateUser(string table, string login)
        {

        }
        
        /// <summary>
        /// 5) Запрос списка всех активных (отсутствует RevokedOn) пользователей, 
        /// список отсортирован по CreatedOn(Доступно Админам)
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IEnumerable<BsonDocument> GetAllActiveUsers(string table)
        {
            var collection = db.GetCollection<User>(table);
            var filter = Builders<User>.Filter.Exists("RevokedOn",false);

            return collection.Find(filter)
                       .Sort(Builders<User>.Sort.Descending("CreatedOn"))
                       .Project("{_id:0, Login:1, Password:1, Name:1}")
                       .ToList();
        }
        /// <summary>
        /// 6) Запрос пользователя по логину, в списке долны быть имя, пол и дата рождения статус активный или нет
        /// (Доступно Админам)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <param name="login"></param>
        /// <returns></returns>
        public User GetUser(string table, string login)
        {
            var collection = db.GetCollection<User>(table);
            var filter = Builders<User>.Filter.Eq("Login", login);

            var user = collection.Find(filter).First();
            return user;
        }
        /// <summary>
        /// 8) Запрос всех пользователей старше определённого возраста 
        /// (Доступно Админам)
        /// </summary>
        /// <param name="table"></param>
        /// <param name="age"></param>
        /// <returns></returns>
        public IEnumerable<BsonDocument> GetUsersOlderThan(string table, int age)
        {
            var birthday = new DateTime(year: (DateTime.Today.Year - age), month: DateTime.Today.Month, day: DateTime.Today.Day);
            var collection = db.GetCollection<User>(table);
            var filter = Builders<User>.Filter.Lt("Birthday", birthday);

            return collection.Find(filter)
                .Project("{_id:0, Login:1, Password:1, Name:1}")
                .ToList();
        }
        /// <summary>
        /// 9) Удаление пользователя по логину полное или мягкое (При мягком удалении должна происходить простановка RevokedOn и RevokedBy) 
        /// (Доступно Админам)
        /// </summary>
        /// <param name="table"></param>
        /// <param name="login"></param>
        /// <param name="isRevocable"></param>
        public void DeleteUser(string table, string login, bool isRevocable)// Вроде ок
        {

            var collection = db.GetCollection<User>(table);
            var filter = Builders<User>.Filter.Eq("Login", login);
            bool isExist = collection.Find(_ => _.Login == login).Any();
            if (!isExist)
            {
                Console.WriteLine("This user doesn't exist");
                return;
            }
            if (!isRevocable)
                collection.DeleteOne(filter);
            else
            {
                collection.UpdateOne(filter, new BsonDocument("$set", new BsonDocument("RevokedBy", "Admin")));
                collection.UpdateOne(filter, new BsonDocument("$set", new BsonDocument("RevokedOn", DateTime.Now)));
            }
        }
        /// <summary>
        /// 10) Восстановление пользователя - Очистка полей (RevokedOn, RevokedBy) 
        /// (Доступно Админам)
        /// </summary>
        /// <param name="table"></param>
        /// <param name="login"></param>
        public void RevokeUser(string table, string login)// Вроде ок
        {
            var collection = db.GetCollection<User>(table);
            var filter = Builders<User>.Filter.Eq("Login", login);

            collection.UpdateOne(filter, Builders<User>.Update.Unset("RevokedOn"));
            collection.UpdateOne(filter, Builders<User>.Update.Unset("RevokedBy"));
        }
    }
}