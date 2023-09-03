using Client.ViewModels.Overtimes;
using server.DTOs.Overtimes;
using server.Models;
using server.Utilities.Handlers;

namespace Client.Contracts
{
    public interface IOvertimeRepository : IRepository<Overtime, Guid>
    {
        public Task<HandlerForResponse<AllRemainingOvertimeDto>> PostOvertime(TestOvertimeDto entity);
    }
}
