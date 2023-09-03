using server.DTOs.Overtimes;
using server.Models;

namespace server.Contracts
{

    public interface IOvertimeRepository : IGeneralRepository<Overtime>
    {
        public string? GetLastOvertimeId();
        public Overtime CreateOvertime(Overtime overtime);
        public IEnumerable<AllRemainingOvertimeDto> RemainingOvertimeList();
        public IEnumerable<AllRemainingOvertimeDto> RemainingOvertimeList(Guid id);
        public IEnumerable<AllRemainingOvertimeDto> RemainingOvertimeByManagerGuidList(Guid managerGuid);
        public AllRemainingOvertimeDto RemainingOvertimeByEmployeeGuid(Guid id);
        public IEnumerable<Overtime>? GetOvertimeByEmployeeGuid(Guid employeeGuid);
        public Overtime? GetOvertimeByOvertimeId(string id);
    }

}
