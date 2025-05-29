namespace BankApp.DTOs
{

    public class AccountDto
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = null!;
        public decimal Balance { get; set; }
        public string AccountType { get; set; } = null!;
        public int UserId { get; set; } 
    }
}
