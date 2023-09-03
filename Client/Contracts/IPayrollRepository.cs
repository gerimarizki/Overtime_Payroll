using Client.ViewModels.Payroll;
using server.Models;
using server.Utilities.Handlers;

namespace Client.Contracts
{
    public interface IPayrollRepository : IRepository<Payroll, Guid>
    {
        //public Task<HandlerForResponse<double>> GetStatistic();
        //public Task<HandlerForResponse<double>> GetStatistic(Guid guid);
    }
}
