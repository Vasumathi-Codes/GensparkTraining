using BankApp.DTOs;
using BankApp.Models;

namespace BankApp.Mappers
{
    public class TransactionMapper
    {
        public TransactionDto MapTransactionToDto(Transaction transaction)
        {
            return new TransactionDto
            {
                Id = transaction.Id,
                AccountId = transaction.AccountId,
                Amount = transaction.Amount,
                TransactionType = transaction.TransactionType,
                Timestamp = transaction.Timestamp
            };
        }

        public Transaction MapDtoToTransaction(TransactionDto dto)
        {
            return new Transaction
            {
                Id = dto.Id,
                AccountId = dto.AccountId,
                Amount = dto.Amount,
                TransactionType = dto.TransactionType,
                Timestamp = dto.Timestamp
            };
        }
    }
}
