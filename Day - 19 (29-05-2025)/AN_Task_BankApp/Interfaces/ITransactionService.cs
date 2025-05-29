using BankApp.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankApp.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionDto> Deposit(int accountId, decimal amount);
        Task<TransactionDto> Withdraw(int accountId, decimal amount);
        Task<IEnumerable<TransactionDto>> GetAllTransactions();
        Task<TransactionDto> GetTransactionById(int id);
        Task<IEnumerable<TransactionDto>> GetTransactionsByAccountId(int accountId);
        Task<(TransactionDto fromTransaction, TransactionDto toTransaction)> TransferFunds(int fromAccountId, int toAccountId, decimal amount);
    }
}
