using server.Models;
using server.Utilities.Handlers;

namespace Client.Contracts
{
    public interface IEmployeeRepository : IRepository<Employee, Guid>
    {
        public Task<HandlerForResponse<int>> GetCount();
    }
}
