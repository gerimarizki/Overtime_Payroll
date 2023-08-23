
using Client.Contracts;
using server.Models;

namespace Client.Repositories
{
    public class HistoryOvertimeRepository : GeneralRepository<HistoryOvertime, Guid>, IHistoryOvertimeRepository
    {
        public HistoryOvertimeRepository(string request = "historiesOvertime/") : base(request)
        {
        }
    }
}