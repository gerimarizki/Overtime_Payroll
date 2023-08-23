using server.Contracts;
using server.Data;
using server.Models;

namespace server.Repositories
{

    public class AccountRepository : GeneralRepository<Account>, IAccountRepository
    {
        public AccountRepository(OvertimeDbContext context) : base(context) { }

        public bool IsDuplicateValue(string value)
        {
            return _context.Set<Account>()
                           .FirstOrDefault(e => e.Email.Contains(value)) is null;
        }

        public Account? GetEmployeeByEmail(string email)
        {
            return _context.Set<Account>().FirstOrDefault(e => e.Email == email);
        }
    }
}