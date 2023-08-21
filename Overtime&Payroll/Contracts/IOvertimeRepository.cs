using Overtime_Payroll.DTOs.Overtimes;
using Overtime_Payroll.Models;

namespace Overtime_Payroll.Contracts
{

    public interface IOvertimeRepository : IGeneralRepository<Overtime>
    {
        public string? GetLastOvertimeId();
        public Overtime CreateOvertime(Overtime overtime);
        public int PaidPerWeekend(int totalHour, double salaryPerHours);
        public IEnumerable<AllRemainingOvertimeDto> RemainingOvertimeList();
        public IEnumerable<AllRemainingOvertimeDto> RemainingOvertimeList(Guid id);
        public IEnumerable<AllRemainingOvertimeDto> RemainingOvertimeByManagerGuidList(Guid managerGuid);
        public AllRemainingOvertimeDto RemainingOvertimeByEmployeeGuid(Guid id);
    }

}
