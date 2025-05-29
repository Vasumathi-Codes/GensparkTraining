namespace BankApp.Models
{
    public class Account
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string AccountNumber { get; set; } = null!;
        public decimal Balance { get; set; }
        public string AccountType { get; set; } = null!; 
        
        public User User { get; set; } = null!;
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
