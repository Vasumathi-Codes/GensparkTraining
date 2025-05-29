using BankApp.DTOs;
using BankApp.Models;

namespace BankApp.Mappers
{
    public class AccountMapper
    {
        public AccountDto MapAccountToDto(Account account)
        {
            return new AccountDto
            {
                Id = account.Id,
                AccountNumber = account.AccountNumber,
                Balance = account.Balance,
                UserId = account.UserId,
                AccountType = account.AccountType
            };
        }

        public Account MapDtoToAccount(AccountDto dto)
        {
            return new Account
            {
                Id = dto.Id,
                AccountNumber = dto.AccountNumber,
                Balance = dto.Balance,
                UserId = dto.UserId,
                AccountType = dto.AccountType
            };
        }
    }
}
