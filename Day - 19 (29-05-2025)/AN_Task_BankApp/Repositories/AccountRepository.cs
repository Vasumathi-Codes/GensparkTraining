using BankApp.Contexts;
using BankApp.Models;
using Microsoft.EntityFrameworkCore;


namespace BankApp.Repositories
{

    public class AccountRepository : Repository<int, Account>
    {
        public AccountRepository(BankingContext bankContext) : base(bankContext)
        {
        }

        public override async Task<Account> Get(int key)
        {
            var account = await _bankContext.Accounts.SingleOrDefaultAsync(a => a.Id == key);
            return account ?? throw new Exception($"No account with given ID {key}");
        }

        public override async Task<IEnumerable<Account>> GetAll()
        {
            var accounts = _bankContext.Accounts;
            if (accounts.Count() == 0)
            {
                throw new Exception("No accounts in the database");
            }
            return await accounts.ToListAsync();
        }

    }
}
