using BankApp.DTOs;
using BankApp.Interfaces;
using BankApp.Models;
using BankApp.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AccountService : IAccountService
{
    private readonly IRepository<int, Account> _accountRepository;
    private readonly IRepository<int, User> _userRepository;
    private readonly AccountMapper _accountMapper;

    public AccountService(IRepository<int, Account> accountRepository, IRepository<int, User> userRepository)
    {
        _accountRepository = accountRepository;
        _userRepository = userRepository;
        _accountMapper = new AccountMapper();
    }

    private string GenerateAccountNumber()
    {
        return Guid.NewGuid().ToString().Replace("-", "").Substring(0, 12).ToUpper(); //3f2504e0-4f89-11d3-9a0c-0305e82c3301 32-char
    }

    public async Task<AccountDto> CreateAccount(CreateAccountDto accountDto)
    {
        var user = await _userRepository.Get(accountDto.UserId);
        if (user == null) throw new Exception("User does not exist");

        var account = new Account
        {
            UserId = accountDto.UserId,
            AccountType = accountDto.AccountType,
            Balance = 0,
            AccountNumber = GenerateAccountNumber()
        };

        var createdAccount = await _accountRepository.Add(account);
        return _accountMapper.MapAccountToDto(createdAccount);
    }

    public async Task<AccountDto> GetAccountById(int id)
    {
        var account = await _accountRepository.Get(id);
        if (account == null) throw new Exception("Account not found");

        return _accountMapper.MapAccountToDto(account);
    }

    public async Task<IEnumerable<AccountDto>> GetAllAccounts()
    {
        var accounts = await _accountRepository.GetAll();
        return accounts.Select(_accountMapper.MapAccountToDto);
    }

    public async Task<IEnumerable<AccountDto>> GetAccountsByUserId(int userId)
    {
        var accounts = await _accountRepository.GetAll();
        var filtered = accounts.Where(a => a.UserId == userId);
        return filtered.Select(_accountMapper.MapAccountToDto);
    }

    public async Task<decimal> CheckBalance(int accountId)
    {
        var account = await _accountRepository.Get(accountId);
        if (account == null)
            throw new Exception("Account not found");

        return account.Balance;
    }
}
