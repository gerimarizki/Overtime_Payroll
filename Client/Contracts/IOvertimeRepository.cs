using Client.Controllers;
using Client.ViewModels.Overtimes;
using server.Models;
using server.Utilities;
using server.Utilities.Handlers;

namespace Client.Contracts
{
    public interface IOvertimeRepository : IRepository<RequestOvertimeViewModel, Guid>
    {
        Task<HandlerForResponse<IEnumerable<RequestOvertimeViewModel>>> GetOvertimeByManager(Guid guid);

        public Task<HandlerForResponse<ApprovalByManager>> GetApproval(ApprovalByManager entity);

    }
}
