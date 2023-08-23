using server.Contracts;
using server.Data;
using server.Models;

namespace server.Repositories
{
    public class RoleRepository : GeneralRepository<Role>, IRoleRepository
    {
        public RoleRepository(OvertimeDbContext context) : base(context) { }

        public Role? GetByName(string name)
        {
            return _context.Set<Role>().FirstOrDefault(r => r.Name == name);
        }
    }
}
