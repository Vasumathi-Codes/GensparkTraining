using System;
using System.Text.RegularExpressions;
using System.Linq;

namespace AN_Task_SolidPrinciples.Single_Responsibility_Principle
{
    // ===== Validators =====
    public class EmailValidator
    {
        public bool Validate(string email) =>
            Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    public class PasswordValidator
    {
        public bool Validate(string password) =>
            password.Length >= 6 && password.Any(char.IsDigit) && password.Any(char.IsUpper);
    }

    // ===== Repository =====
    public class UserRepository
    {
        public void Save(string username, string password, string email)
        {
            Console.WriteLine($"[DB] Saving user: {username}...");
        }
    }

    // ===== Mail Service =====
    public class EmailSender
    {
        public void SendWelcomeEmail(string email)
        {
            Console.WriteLine($"[MAIL] Sending welcome email to: {email}");
        }
    }

    // ===== Logger =====
    public class Logger
    {
        public void Log(string message) =>
            Console.WriteLine($"[LOG] {DateTime.Now}: {message}");
    }

    // ===== Service Layer =====
    public class UserService
    {
        private readonly EmailValidator _emailValidator;
        private readonly PasswordValidator _passwordValidator;
        private readonly UserRepository _userRepository;
        private readonly EmailSender _emailSender;
        private readonly Logger _logger;

        public UserService(
            EmailValidator emailValidator,
            PasswordValidator passwordValidator,
            UserRepository userRepository,
            EmailSender emailSender,
            Logger logger)
        {
            _emailValidator = emailValidator;
            _passwordValidator = passwordValidator;
            _userRepository = userRepository;
            _emailSender = emailSender;
            _logger = logger;
        }

        public void Register(string username, string password, string email)
        {
            if (!_emailValidator.Validate(email))
            {
                Console.WriteLine("Invalid email.");
                return;
            }

            if (!_passwordValidator.Validate(password))
            {
                Console.WriteLine("Weak password.");
                return;
            }

            _userRepository.Save(username, password, email);
            _emailSender.SendWelcomeEmail(email);
            _logger.Log($"User {username} registered.");
        }
    }

}
    
