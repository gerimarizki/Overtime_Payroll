using Overtime_Payroll.Models;

namespace Overtime_Payroll.Contracts
{
    public interface IRoleRepository : IGeneralRepository<Role>
    {
        Role? GetByName(string name);
    }
}
