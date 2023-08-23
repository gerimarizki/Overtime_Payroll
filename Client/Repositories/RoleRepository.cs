
using Client.Contracts;
using server.Models;

namespace Client.Repositories
{
    public class RoleRepository : GeneralRepository<Role, Guid>, IRoleRepository
    {
        public RoleRepository(string request = "roles/") : base(request)
        {
        }
    }
}