using Overtime_Payroll.Contracts;
using Overtime_Payroll.Data;
using Overtime_Payroll.Models;

namespace Overtime_Payroll.Repositories
{
    public class PayslipRepository : GeneralRepository<Payroll>, IPayrollRepository
    {
        public PayslipRepository(OvertimeDbContext context) : base(context) { }
    }
}