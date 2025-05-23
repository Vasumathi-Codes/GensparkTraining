using System;
using System.Collections.Generic;
using AN_Task_ProxyPattern.Models;
using AN_Task_ProxyPattern.Interfaces;
using AN_Task_ProxyPattern.Core;
using AN_Task_ProxyPattern.Helpers;

namespace AN_Task_ProxyPattern
{
    class Program
    {
        static Dictionary<string, User> users = new Dictionary<string, User>();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\nChoose an option:");
                Console.WriteLine("1. Add User");
                Console.WriteLine("2. Read File");
                Console.WriteLine("3. Exit");
                Console.Write("Enter choice: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddUser();
                        break;

                    case "2":
                        PerformFileOperation();
                        break;

                    case "3":
                        Console.WriteLine("Exiting program...");
                        return;

                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }

        static void AddUser()
        {
            string username = InputHelper.ReadNonEmptyString("Enter username: ");

            string role = InputHelper.ReadNonEmptyString("Enter role (Admin/User/Guest): ");

            if (!IsValidRole(role))
            {
                Console.WriteLine("Invalid role. Must be Admin, User, or Guest.");
                return;
            }

            users[username.ToLower()] = new User(username, role);
            Console.WriteLine($"User '{username}' with role '{role}' added successfully.");
        }

        static void PerformFileOperation()
        {
            string username = InputHelper.ReadNonEmptyString("Enter your username: ");

            if (!users.ContainsKey(username.ToLower()))
            {
                Console.WriteLine("User not found. Please add the user first.");
                return;
            }

            User currentUser = users[username.ToLower()];
            IFile proxyFile = new ProxyFile("C:\\Users\\VC\\source\\repos\\AN-Task-ProxyPattern\\AN-Task-ProxyPattern\\ConfidentialFile.txt", currentUser);

            Console.WriteLine($"\nUser: {currentUser.UserName} | Role: {currentUser.Role}");
            proxyFile.Read();
        }

        static bool IsValidRole(string role)
        {
            string lowerRole = role.Trim().ToLower();
            return lowerRole == "admin" || lowerRole == "user" || lowerRole == "guest";
        }

    }
}
