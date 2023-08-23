using server.Models;

namespace Client.Contracts
{
    public interface IPayrollRepository : IRepository<Payroll, Guid>
    {
    }
}
