using server.Models;

namespace server.Contracts
{

    public interface IPayrollRepository : IGeneralRepository<Payroll>
    {
        Payroll? GetLastPayroll(Guid guid);
    }

}
