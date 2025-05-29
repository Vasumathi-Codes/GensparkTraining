namespace BankApp.DTOs
{
    public class TransferFundsDto
    {
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
    }

}
