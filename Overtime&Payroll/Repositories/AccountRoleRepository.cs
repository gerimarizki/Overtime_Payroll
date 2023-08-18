using Overtime_Payroll.Contracts;
using Overtime_Payroll.Data;
using Overtime_Payroll.Models;

namespace Overtime_Payroll.Repositories
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