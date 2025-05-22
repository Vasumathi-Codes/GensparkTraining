using System;
using System.Text.RegularExpressions;

namespace AN_Task_SolidPrinciples.Single_Responsibility_Principle
{
    internal class SRP_BadExample
    {
        public class UserService
        {
            public void Register(string username, string password, string email)
            {
                if (!ValidateEmail(email))
                {
                    Console.WriteLine("Invalid email.");
                    return;
                }

                if (!ValidatePassword(password))
                {
                    Console.WriteLine("Weak password.");
                    return;
                }

                SaveToDatabase(username, password, email);
                SendWelcomeEmail(email);
                LogAction($"User {username} registered.");
            }

            private bool ValidateEmail(string email) =>
                Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            private bool ValidatePassword(string password) =>
                password.Length >= 6 && password.Any(char.IsDigit) && password.Any(char.IsUpper);

            private void SaveToDatabase(string username, string password, string email) =>
                Console.WriteLine($"Saving user: {username} to database...");

            private void SendWelcomeEmail(string email) =>
                Console.WriteLine($"Sending welcome email to: {email}");

            private void LogAction(string message) =>
                Console.WriteLine($"[LOG] {DateTime.Now}: {message}");
        }
    }
}
