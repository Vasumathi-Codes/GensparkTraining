namespace BankApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        
        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
