using Overtime_Payroll.Models;

namespace Overtime_Payroll.Contracts
{
    public interface IAccountRoleRepository : IGeneralRepository<AccountRole>
    {
        IEnumerable<AccountRole> GetAccountRolesByAccountGuid(Guid guid);

        public AccountRole GetAccountRoles(Guid guid);
    }
}
