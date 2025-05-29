namespace BankApp.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; } = null!; 
        public DateTime Timestamp { get; set; }
        
        public Account Account { get; set; } = null!;
    }
}
