using server.Contracts;
using server.DTOs.HistoriesOvertime;
using server.DTOs.Overtimes;
using server.Models;
using server.Utilities.Enums;
using server.Utilities.Handlers;

namespace server.Services
{

    //Overtime Service by Geri
    public class OvertimeService
    {
        private readonly IOvertimeRepository _overtimeRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPayrollRepository _payrollRepository;
        private readonly IHistoryOvertimeRepository _historyOvertimeRepository;

        public OvertimeService(IOvertimeRepository overtimeRepository, IEmployeeRepository employeeRepository, IPayrollRepository payrollRepository, IHistoryOvertimeRepository historyOvertimeRepository)
        {
            _overtimeRepository = overtimeRepository;
            _employeeRepository = employeeRepository;
            _payrollRepository = payrollRepository;
            _historyOvertimeRepository = historyOvertimeRepository;
        }

        //----------------------------
        //Tambahan data 29/08/2023 count status
        //----------------------------
        public GetCountedStatusDto CountStatus()
        {
            var status = _overtimeRepository.GetAll();
            var overtime = new GetCountedStatusDto();
            foreach (var item in status)
            {
                if (item.Status == StatusLevel.Accepted)
                {
                    overtime.CountAccepted++;
                }
                else if (item.Status == StatusLevel.Rejected)
                {
                    overtime.CountRejected++;
                }
                else
                {
                    overtime.CountWaiting++;
                }

            }

            return overtime;
        }
        //--------------------------------------
        //tutup
        //--------------------------------------


        //gagal
        //public IEnumerable<GetOvertimeDto> GetOvertime()
        //{
        //    var overtime = (from e in _employeeRepository.GetAll()
        //                    join o in _overtimeRepository.GetAll() on e.Guid equals o.EmployeeGuid
        //                    where e.Guid == o.EmployeeGuid
        //                    select new GetOvertimeDto
        //                    {
        //                        Guid = o.Guid,
        //                        OvertimeId = o.OvertimeId,
        //                        StartOvertimeDate = DateTime.Now,
        //                        EndOvertimeDate= DateTime.Now,
        //                        Remarks = o.Remarks,
        //                        Status = o.Status,
        //                        EmployeeGuid = o.EmployeeGuid,
        //                        FullName = e.FirstName+ " "+e.LastName,
        //                        Remaining = o.OvertimeRemaining,
        //                        Paid = o.PaidOvertime,
        //                        CreatedDate = DateTime.Now,
        //                    }).ToList();
            //var overtime = _overtimeRepository.GetAll().ToList();
            //if (!overtime.Any()) return Enumerable.Empty<GetOvertimeDto>();
            //List<GetOvertimeDto> overtimeDTO = new();

                            //foreach (var over in overtime)
                            //{
                            //    overtimeDTO.Add((GetOvertimeDto)over);
                            //}

                            //return overtimeDTO;
        //     if (!overtime.Any())
        //    {
        //        return Enumerable.Empty<GetOvertimeDto>();
        //    }
        //    return overtime;
        //}

        //tambahan baru 24/08/2023 dibuat karena create harus buat penambahan
        public AllRemainingOvertimeDto? CreateOvertimeToEmployee(TestOvertimeDto ovt)
        {
            Models.Overtime overtime = ovt;
            overtime.OvertimeId = HandlerGenerator.OvertimeId(_overtimeRepository.GetLastOvertimeId());
            overtime.StartOvertimeDate = ovt.StartOvertimeDate;
            overtime.EndOvertimeDate = ovt.EndOvertimeDate;
            overtime.Status = StatusLevel.Waiting;
            overtime.CreatedDate = DateTime.Now;
            var createdOver = _overtimeRepository.CreateOvertime(overtime);
            var createHistory = _historyOvertimeRepository.Create(new HistoryOvertime
            {
                OvertimeGuid = createdOver.Guid,
                Guid = new Guid(),
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            }) ;
            if (createdOver is null) return null;

            return (AllRemainingOvertimeDto)createdOver;

        }

        //get all data testing ger
        public IEnumerable<AllRemainingOvertimeDto>? GetAllTestOvertimeToEmployee()
        {

            var master = (from o in _overtimeRepository.GetAll()
                          join e in _employeeRepository.GetAll() on o.EmployeeGuid equals e.Guid
                          select new AllRemainingOvertimeDto
                          {
                              Guid = o.Guid,
                              FullName = e.FirstName + " " + e.LastName,
                              EmployeeGuid = e.Guid,
                              EndOvertimeDate = o.EndOvertimeDate,
                              OvertimeId = o.OvertimeId,
                              PaidOvertime = o.PaidOvertime,
                              OvertimeRemaining = o.OvertimeRemaining,
                              Remarks = o.Remarks,
                              StartOvertimeDate = o.StartOvertimeDate,
                              Status = o.Status,
                              CreatedDate = o.StartOvertimeDate,
                          }).ToList();
            return master;
        }

        //tutup


        public GetOvertimeDto? GetOvertimeByGuid(Guid guid)
        {
            var overtime = _overtimeRepository.GetByGuid(guid);
            if (overtime is null) return null;

            return (GetOvertimeDto)overtime;
        }


        public GetOvertimeDto? CreateOvertime(CreateOvertimeDto newOvertime)
        {
            Models.Overtime overtime = newOvertime;
            overtime.OvertimeId = HandlerGenerator.OvertimeId(_overtimeRepository.GetLastOvertimeId());
            var createdOvertime = _overtimeRepository.Create(overtime);
            if (createdOvertime is null) return null;

            return (GetOvertimeDto)createdOvertime;
        }

        //gagal
        //public GetOvertimeDto? CreateOvertime(CreateOvertimeDto newOvertime)
        //{
        //    Models.Overtime overtime = newOvertime;
        //    overtime.OvertimeId = HandlerGenerator.OvertimeId(_overtimeRepository.GetLastOvertimeId());
        //    overtime.Guid = new Guid();
        //    var createdOvertime = _overtimeRepository.Create(overtime);
        //    if (createdOvertime is null) return null;


        //    var overtimeDetails = (from e in _employeeRepository.GetAll()
        //                           join o in _overtimeRepository.GetAll() on e.Guid equals createdOvertime.EmployeeGuid
        //                           where e.Guid == createdOvertime.EmployeeGuid
        //                           select new GetOvertimeDto
        //                           {
        //                               Guid = o.Guid,
        //                               OvertimeId = createdOvertime.OvertimeId,
        //                               StartOvertimeDate = createdOvertime.StartOvertimeDate,
        //                               EndOvertimeDate = createdOvertime.EndOvertimeDate,
        //                               Remarks = createdOvertime.Remarks,
        //                               Status = createdOvertime.Status,
        //                               EmployeeGuid = createdOvertime.EmployeeGuid,
        //                               FullName = e.FirstName + " " + e.LastName,
        //                               Remaining = createdOvertime.OvertimeRemaining,
        //                               Paid = createdOvertime.PaidOvertime,
        //                               CreatedDate = createdOvertime.CreatedDate
        //                           }).FirstOrDefault();

        //    return overtimeDetails;
        //}


        public int UpdateOvertime(UpdateOvertimeDto updateOvertime)
        {
            var getOvertime = _overtimeRepository.GetByGuid(updateOvertime.Guid);
            if (getOvertime is null) return -1;

            Overtime updateovertime = updateOvertime;
            updateovertime.CreatedDate = getOvertime.CreatedDate;
            var isUpdate = _overtimeRepository.Update(updateOvertime);
            return !isUpdate ? 0 : 1;
        }
        public int DeleteOvertime(Guid guid)
        {
            var overtime = _overtimeRepository.GetByGuid(guid);
            if (overtime == null) { return -1; }

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

        // check status employee by manager to accepted/rejected ger 
        public int UpdateStatusRemaining(UpdateOvertimeStatus overtime)
        {
            var over = _overtimeRepository.GetByGuid(overtime.Guid);
            if (over is null) return -1;
            over.Guid = overtime.Guid;
            over.Status = overtime.Status;
            over.CreatedDate = over.CreatedDate;
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
