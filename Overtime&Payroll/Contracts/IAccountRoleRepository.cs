using server.Models;

namespace server.Contracts
{
    public interface IAccountRoleRepository : IGeneralRepository<AccountRole>
    {
        IEnumerable<AccountRole> GetAccountRolesByAccountGuid(Guid guid);

        public AccountRole GetAccountRoles(Guid guid);
    }
}
