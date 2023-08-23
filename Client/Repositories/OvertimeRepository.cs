
using Client.Contracts;
using server.Models;

namespace Client.Repositories
{
    public class OvertimeRepository : GeneralRepository<Overtime, Guid>, IOvertimeRepository
    {
        public OvertimeRepository(string request = "overtimes/") : base(request)
        {
        }
    }
}