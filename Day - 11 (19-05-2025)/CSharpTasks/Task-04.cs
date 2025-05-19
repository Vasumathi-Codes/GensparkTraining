using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTasks
{
    internal class Task_04
    {
        // 4) Take username and password from user. Check if user name is "Admin" and password is "pass" if yes then print success message.
        // Give 3 attempts to user. In the end of eh 3rd attempt if user still is unable to provide valid creds then exit the application after print the message 
        // "Invalid attempts for 3 times. Exiting...."

        public void Run()
        {
            const string adminUsername = "Admin";
            const string adminPassword = "pass";

            int currentAttempt = 0;
            int maxAttempt = 3;

            while (currentAttempt < maxAttempt)
            {
                Console.Write("Enter username: ");
                string? username = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(username))
                {
                    Console.WriteLine("Username cannot be empty. Try again.");
                    continue;
                }

                Console.Write("Enter password: ");
                string? password = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(password))
                {
                    Console.WriteLine("Password cannot be empty. Try again.");
                    continue;
                }

                if (username == adminUsername && password == adminPassword)
                {
                    Console.WriteLine("Login successful! Welcome Admin.");
                    return;
                }
                else
                {
                    currentAttempt++;
                    Console.WriteLine($"Invalid username or password. Attempts left: {maxAttempt - currentAttempt}");
                }
            }
            Console.WriteLine("Invalid attempts for 3 times. Exiting....");
        }
    }
}
