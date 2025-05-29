using BankApp.DTOs;
using BankApp.Interfaces;
using BankApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApp.Mappers;
using BankApp.Contexts;

public class TransactionService : ITransactionService
{
    private readonly IRepository<int, Transaction> _transactionRepository;
    private readonly IRepository<int, Account> _accountRepository;
    private readonly TransactionMapper _transactionMapper;
    private readonly BankingContext _dbContext;

    public TransactionService(IRepository<int, Transaction> transactionRepository,
                              IRepository<int, Account> accountRepository,
                              BankingContext dbContext)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
        _transactionMapper = new TransactionMapper();
        _dbContext = dbContext;
    }

    public async Task<TransactionDto> Deposit(int accountId, decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Deposit amount must be positive");

        var account = await _accountRepository.Get(accountId)
                      ?? throw new KeyNotFoundException("Account not found");

        account.Balance += amount;
        await _accountRepository.Update(accountId, account);

        var transaction = new Transaction
        {
            AccountId = accountId,
            Amount = amount,
            TransactionType = "Deposit",
            Timestamp = DateTime.UtcNow
        };

        var createdTransaction = await _transactionRepository.Add(transaction);
        return _transactionMapper.MapTransactionToDto(createdTransaction);
    }

    public async Task<TransactionDto> Withdraw(int accountId, decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Withdrawal amount must be positive");

        var account = await _accountRepository.Get(accountId)
                      ?? throw new KeyNotFoundException("Account not found");

        if (account.Balance < amount)
            throw new InvalidOperationException("Insufficient balance");

        account.Balance -= amount;
        await _accountRepository.Update(accountId, account);

        var transaction = new Transaction
        {
            AccountId = accountId,
            Amount = amount,
            TransactionType = "Withdraw",
            Timestamp = DateTime.UtcNow
        };

        var createdTransaction = await _transactionRepository.Add(transaction);
        return _transactionMapper.MapTransactionToDto(createdTransaction);
    }

    public async Task<TransactionDto> GetTransactionById(int id)
    {
        var transaction = await _transactionRepository.Get(id)
                           ?? throw new KeyNotFoundException("Transaction not found");

        return _transactionMapper.MapTransactionToDto(transaction);
    }

    public async Task<IEnumerable<TransactionDto>> GetAllTransactions()
    {
        var transactions = await _transactionRepository.GetAll();
        return transactions.Select(t => _transactionMapper.MapTransactionToDto(t));
    }

    public async Task<IEnumerable<TransactionDto>> GetTransactionsByAccountId(int accountId)
    {
        var transactions = await _transactionRepository.GetAll();
        var filtered = transactions.Where(t => t.AccountId == accountId);
        return filtered.Select(t => _transactionMapper.MapTransactionToDto(t));
    }

    public async Task<(TransactionDto fromTransaction, TransactionDto toTransaction)> TransferFunds(int fromAccountId, int toAccountId, decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Transfer amount must be positive");

        var fromAccount = await _accountRepository.Get(fromAccountId)
                        ?? throw new KeyNotFoundException("Source account not found");

        var toAccount = await _accountRepository.Get(toAccountId)
                        ?? throw new KeyNotFoundException("Destination account not found");

        if (fromAccount.Balance < amount)
            throw new InvalidOperationException("Insufficient balance in source account");

        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            fromAccount.Balance -= amount;
            toAccount.Balance += amount;

            await _accountRepository.Update(fromAccountId, fromAccount);
            await _accountRepository.Update(toAccountId, toAccount);

            var fromTransaction = new Transaction
            {
                AccountId = fromAccountId,
                Amount = amount,
                TransactionType = "Transfer Out",
                Timestamp = DateTime.UtcNow
            };

            var toTransaction = new Transaction
            {
                AccountId = toAccountId,
                Amount = amount,
                TransactionType = "Transfer In",
                Timestamp = DateTime.UtcNow
            };

            var createdFromTransaction = await _transactionRepository.Add(fromTransaction);
            var createdToTransaction = await _transactionRepository.Add(toTransaction);

            await transaction.CommitAsync();

            return (_transactionMapper.MapTransactionToDto(createdFromTransaction), _transactionMapper.MapTransactionToDto(createdToTransaction));
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

}
