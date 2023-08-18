using Microsoft.EntityFrameworkCore.Migrations;
using Overtime_Payroll.Contracts;
using Overtime_Payroll.Data;
using Overtime_Payroll.Models;

namespace Overtime_Payroll.Repositories
{
    public class HistoryRepository : GeneralRepository<HistoryOvertime>, IHistoryOvertimeRepository
    {
        public HistoryRepository(OvertimeDbContext context) : base(context) { }
    }
}