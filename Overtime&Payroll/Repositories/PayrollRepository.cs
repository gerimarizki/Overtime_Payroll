using server.Contracts;
using server.Data;
using server.Models;

namespace server.Repositories
{
    public class PayrollRepository : GeneralRepository<Payroll>, IPayrollRepository
    {
        public PayrollRepository(OvertimeDbContext context) : base(context) { }
    }
}