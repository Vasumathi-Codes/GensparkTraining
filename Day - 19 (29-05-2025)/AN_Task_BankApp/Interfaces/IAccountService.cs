using BankApp.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankApp.Interfaces
{
    public interface IAccountService
    {
        Task<AccountDto> CreateAccount(CreateAccountDto accountDto);
        Task<AccountDto> GetAccountById(int id);
        Task<IEnumerable<AccountDto>> GetAllAccounts();
        Task<IEnumerable<AccountDto>> GetAccountsByUserId(int userId);
        Task<decimal> CheckBalance(int accountId);
    }
}
