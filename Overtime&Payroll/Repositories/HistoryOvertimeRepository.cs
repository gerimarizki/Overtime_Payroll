using Microsoft.EntityFrameworkCore.Migrations;
using Overtime_Payroll.Contracts;
using Overtime_Payroll.Data;
using Overtime_Payroll.Models;

namespace Overtime_Payroll.Repositories
{
    public class HistoryOvertimeRepository : GeneralRepository<HistoryOvertime>, IHistoryOvertimeRepository
    {
        public HistoryOvertimeRepository(OvertimeDbContext context) : base(context) { }
    }
}