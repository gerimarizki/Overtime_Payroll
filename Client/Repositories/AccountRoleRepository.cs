
using Client.Contracts;
using server.Models;

namespace Client.Repositories
{
    public class AccountRoleRepository : GeneralRepository<AccountRole, Guid>, IAccountRoleRepository
    {
        public AccountRoleRepository(string request = "account-roles/") : base(request)
        {
        }
    }
}