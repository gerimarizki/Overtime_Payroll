using Microsoft.EntityFrameworkCore.Migrations;
using server.Contracts;
using server.DTOs.HistoriesOvertime;

namespace server.Services
{
    public class HistoryOvertimeService
    {
        private readonly IHistoryOvertimeRepository _historyOvertimeRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IOvertimeRepository _overtimeRepository;

        public HistoryOvertimeService(IHistoryOvertimeRepository historyOvertimeRepository, IEmployeeRepository employeeRepository, IOvertimeRepository overtimeRepository)
        {
            _historyOvertimeRepository = historyOvertimeRepository;
            _employeeRepository = employeeRepository;
            _overtimeRepository = overtimeRepository;
        }

        public IEnumerable<GetAllHistoryOvertimeDto> GetAllHistoriesOvertime()
        {
            var master = (from history in _historyOvertimeRepository.GetAll()
                          join overtime in _overtimeRepository.GetAll() on history.OvertimeGuid equals overtime.Guid
                          join employee in _employeeRepository.GetAll() on overtime.EmployeeGuid equals employee.Guid
                          select new GetAllHistoryOvertimeDto
                          {
                              Guid = history.Guid,
                              OvertimeId = overtime.OvertimeId,
                              FullName = employee.FirstName + " " + employee.LastName,
                              NIK = employee.NIK,
                              StartOvertimeDate = overtime.StartOvertimeDate,
                              EndOvertimeDate = overtime.EndOvertimeDate,
                              Submitted = DateTime.Now,
                              Status = overtime.Status,
                              ManagerGuid = employee.ManagerGuid

                          }).ToList();

            foreach (var getDataEmployee in master)
            {
                if (getDataEmployee.ManagerGuid != Guid.Empty)
                {
                    var manager = master.FirstOrDefault(e => e.Guid == getDataEmployee.ManagerGuid);
                    if (manager != null)
                    {
                        getDataEmployee.ApproveBy = $"{manager.NIK} - {manager.FullName}";
                    }
                }
            }

            return master;
        }


        public IEnumerable<GetHistoryOvertime> GetHistoryOvertime()
        {
            var histories = _historyOvertimeRepository.GetAll().ToList();
            if (!histories.Any()) return Enumerable.Empty<GetHistoryOvertime>(); // 404
            List<GetHistoryOvertime> historyDtos = new();

            foreach (var history in histories)
            {
                historyDtos.Add((GetHistoryOvertime)history);
            }

            return historyDtos; // ada
        }

        public GetHistoryOvertime? GetHistoryOvertimeByGuid(Guid guid)
        {
            var history = _historyOvertimeRepository.GetByGuid(guid);

            if (history is null) return null;

            return (GetHistoryOvertime)history;
        }

        public GetHistoryOvertime? CreateHistoryOvertime(CreateHistoryOvertimeDto newHistoryDto)
        {
            var createdHistory = _historyOvertimeRepository.Create(newHistoryDto);
            if (createdHistory is null) return null;

            return (GetHistoryOvertime)createdHistory;
        }

        public int UpdateHistoryOvertime(UpdateHistoryOvertimeDto updateHistoryDto)
        {
            var getHistory = _historyOvertimeRepository.GetByGuid(updateHistoryDto.Guid);

            if (getHistory is null) return -1;

            var isUpdated = _historyOvertimeRepository.Update(updateHistoryDto);
            return !isUpdated ? 0 :
                1;
        }

        public int DeleteHistoryOvertime(Guid guid)
        {
            var history = _historyOvertimeRepository.GetByGuid(guid);

            if (history is null) return -1;

            var isDeleted = _historyOvertimeRepository.Delete(history);
            return !isDeleted ? 0 :
                1;
        }
    }
}
