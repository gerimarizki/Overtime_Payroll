
using Client.Contracts;
using server.Models;
using server.Repositories;

namespace Client.Repositories
{
    public class PayrollRepository : GeneralRepository<Payroll, Guid>, IPayrollRepository
    {
        public PayrollRepository(string request = "payrolls/") : base(request)
        {
        }
    }
}