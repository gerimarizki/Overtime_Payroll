using Overtime_Payroll.Contracts;
using Overtime_Payroll.Data;
using Overtime_Payroll.Models;

namespace Overtime_Payroll.Repositories
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
