using server.Contracts;
using server.DTOs.Overtimes;
using server.DTOs.Payrolls;
using server.Migrations;
using server.Models;
using server.Utilities.Handlers;

namespace server.Services
{
    public class OvertimeService
    {
        private readonly IOvertimeRepository _overtimeRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public OvertimeService(IOvertimeRepository overtimeRepository, IEmployeeRepository employeeRepository)
        {
            _overtimeRepository = overtimeRepository;
            _employeeRepository = employeeRepository;
        }

        public IEnumerable<GetOvertimeDto> GetOvertime()
        {
            var overtime = (from e in _employeeRepository.GetAll()
                            join o in _overtimeRepository.GetAll() on e.Guid equals o.EmployeeGuid
                            where e.Guid == o.EmployeeGuid
                            select new GetOvertimeDto
                            {
                                Guid = e.Guid,
                                OvertimeId = o.OvertimeId,
                                StartOvertimeDate = DateTime.Now,
                                EndOvertimeDate= DateTime.Now,
                                Remarks = o.Remarks,
                                Status = o.Status,
                                EmployeeGuid = o.EmployeeGuid,
                                FullName = e.FirstName+ " "+e.LastName,
                                Remaining = o.OvertimeRemaining,
                                Paid = o.PaidOvertime,
                                CreatedDate = DateTime.Now,
                            }).ToList();
            //var overtime = _overtimeRepository.GetAll().ToList();
            //if (!overtime.Any()) return Enumerable.Empty<GetOvertimeDto>();
            //List<GetOvertimeDto> overtimeDTO = new();

                            //foreach (var over in overtime)
                            //{
                            //    overtimeDTO.Add((GetOvertimeDto)over);
                            //}

                            //return overtimeDTO;
             if (!overtime.Any())
            {
                return Enumerable.Empty<GetOvertimeDto>();
            }
            return overtime;
        }

        public GetOvertimeDto? GetOvertimeByGuid(Guid guid)
        {
            var overtime = _overtimeRepository.GetByGuid(guid);
            if (overtime is null) return null;

            return (GetOvertimeDto)overtime;
        }


        //public GetOvertimeDto? CreateOvertime(CreateOvertimeDto newOvertime)
        //{
        //    Models.Overtime overtime = newOvertime;
        //    overtime.OvertimeId = HandlerGenerator.OvertimeId(_overtimeRepository.GetLastOvertimeId());
        //    var createdOvertime = _overtimeRepository.Create(overtime);
        //    if (createdOvertime is null) return null;

        //    return (GetOvertimeDto)createdOvertime;
        //}

        public GetOvertimeDto? CreateOvertime(CreateOvertimeDto newOvertime)
        {
            Models.Overtime overtime = newOvertime;
            overtime.OvertimeId = HandlerGenerator.OvertimeId(_overtimeRepository.GetLastOvertimeId());
            var createdOvertime = _overtimeRepository.Create(overtime);
            if (createdOvertime is null) return null;

            var overtimeDetails = (from e in _employeeRepository.GetAll()
                                   join o in _overtimeRepository.GetAll() on e.Guid equals createdOvertime.EmployeeGuid
                                   where e.Guid == createdOvertime.EmployeeGuid
                                   select new GetOvertimeDto
                                   {
                                       Guid = e.Guid,
                                       OvertimeId = createdOvertime.OvertimeId,
                                       StartOvertimeDate = createdOvertime.StartOvertimeDate,
                                       EndOvertimeDate = createdOvertime.EndOvertimeDate,
                                       Remarks = createdOvertime.Remarks,
                                       Status = createdOvertime.Status,
                                       EmployeeGuid = createdOvertime.EmployeeGuid,
                                       FullName = e.FirstName + " " + e.LastName,
                                       Remaining = createdOvertime.OvertimeRemaining,
                                       Paid = createdOvertime.PaidOvertime,
                                       CreatedDate = createdOvertime.CreatedDate
                                   }).FirstOrDefault();

            return overtimeDetails;
        }


        public int UpdateOvertime(UpdateOvertimeDto updateOvertime)
        {
            var getOvertime = _overtimeRepository.GetByGuid(updateOvertime.Guid);
            if (getOvertime is null) return -1;

            var isUpdate = _overtimeRepository.Update(updateOvertime);
            return !isUpdate ? 0 : 1;
        }
        public int DeleteOvertime(Guid guid)
        {
            var overtime = _overtimeRepository.GetByGuid(guid);
            if (overtime is null) return -1;

            var isDelete = _overtimeRepository.Delete(overtime);
            return !isDelete ? 0 : 1;
        }


        public IEnumerable<AllRemainingOvertimeDto>? GetAllByGuidEmp(Guid guid)
        {
            var employee = _employeeRepository.GetByGuid(guid);
            var overtime = _overtimeRepository.RemainingOvertimeList(employee.Guid);
            if (overtime is null) return null;

            return overtime;
        }
        public IEnumerable<AllRemainingOvertimeDto>? GetAllByGuidManager(Guid guid)
        {
            var manager = _employeeRepository.GetByGuid(guid);
            var overtime = _overtimeRepository.RemainingOvertimeByManagerGuidList(manager.Guid);
            if (overtime is null) return null;
            return overtime;
        }
        public int UpdateStatusRemaining(UpdateOvertimeStatus overtime)
        {
            var over = _overtimeRepository.GetByGuid(overtime.Guid);
            if (over is null) return -1;
            over.Guid = overtime.Guid;
            over.Status = overtime.Status;
            if (over.Status == Utilities.Enums.StatusLevel.Rejected)
            {
                over.OvertimeRemaining += Convert.ToInt32((over.EndOvertimeDate - over.StartOvertimeDate).TotalHours);
                over.PaidOvertime = 0;
            }
            else if (over.Status == Utilities.Enums.StatusLevel.Accepted || over.Status == Utilities.Enums.StatusLevel.Waiting)
            {
                over.OvertimeRemaining += 0;
            }
            var isUpdate = _overtimeRepository.Update(over);
            return !isUpdate ? 0 : 1;

        }



    }
}
