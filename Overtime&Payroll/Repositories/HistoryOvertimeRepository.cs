using Microsoft.EntityFrameworkCore.Migrations;
using server.Contracts;
using server.Data;
using server.Models;

namespace server.Repositories
{
    public class HistoryOvertimeRepository : GeneralRepository<HistoryOvertime>, IHistoryOvertimeRepository
    {
        public HistoryOvertimeRepository(OvertimeDbContext context) : base(context) { }
    }
}