namespace BankApp.DTOs
{
    public class CreateAccountDto
    {
        public int UserId { get; set; }
        public string AccountType { get; set; } = null!;
    }
}
