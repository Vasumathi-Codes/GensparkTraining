using System;

namespace AN_Task_SolidPrinciples.Dependency_Inversion_Principle
{
    internal class DIP_BadExample
    {
        public class MySqlDatabase
        {
            public void Save(string data)
            {
                Console.WriteLine("Saving to MySQL Database: " + data);
            }
        }

        public class UserService
        {
            private MySqlDatabase _database = new MySqlDatabase();

            public void SaveUser(string user)
            {
                _database.Save(user);  // High-level module depends on low-level concrete class
            }
        }

        public static void Run()
        {
            var service = new UserService();
            service.SaveUser("User1");
        }
    }
}
