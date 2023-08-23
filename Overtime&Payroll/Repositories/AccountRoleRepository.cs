using server.Contracts;
using server.Data;
using server.Models;

namespace server.Repositories
{
    public class AccountRoleRepository : GeneralRepository<AccountRole>, IAccountRoleRepository
    {
        public AccountRoleRepository(OvertimeDbContext context) : base(context) { }

        public IEnumerable<AccountRole> GetAccountRolesByAccountGuid(Guid guid)
        {
            return _context.Set<AccountRole>().Where(ar => ar.AccountGuid == guid);
        }

        public AccountRole GetAccountRoles(Guid guid)
        {
            var entity = _context.Set<AccountRole>().FirstOrDefault(x => x.AccountGuid == guid);

            _context.ChangeTracker.Clear();
            return entity;
        }
    }
}