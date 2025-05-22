using System;

namespace AN_Task_SolidPrinciples.Dependency_Inversion_Principle
{
    internal class DIP_GoodPractice
    {
        // Abstraction (High-level and low-level depend on this)
        public interface IDatabase
        {
            void Save(string data);
        }

        // Low-level module implementing abstraction
        public class MySqlDatabase : IDatabase
        {
            public void Save(string data)
            {
                Console.WriteLine("Saving to MySQL Database: " + data);
            }
        }

        // Low-level module implementing abstraction
        public class MongoDatabase : IDatabase
        {
            public void Save(string data)
            {
                Console.WriteLine("Saving to MongoDB Database: " + data);
            }
        }

        // High-level module depends on abstraction, not concrete
        public class UserService
        {
            private readonly IDatabase _database;

            // Dependency injected via constructor
            public UserService(IDatabase database)
            {
                _database = database;
            }

            public void SaveUser(string user)
            {
                _database.Save(user);
            }
        }

        public static void Run()
        {
            IDatabase Db = new MySqlDatabase();
            UserService userService = new UserService(Db);
            userService.SaveUser("User1");

            Db = new MongoDatabase();
            UserService userService2 = new UserService(Db);
            userService2.SaveUser("User2");
        }
    }
}
