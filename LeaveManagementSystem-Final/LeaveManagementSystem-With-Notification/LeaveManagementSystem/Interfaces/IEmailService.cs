namespace LeaveManagementSystem.Interfaces {
public interface IEmailService
{
    Task SendAsync(string to, string subject, string body);
}
}
