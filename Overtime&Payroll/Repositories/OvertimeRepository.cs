using Overtime_Payroll.Contracts;
using Overtime_Payroll.Data;
using Overtime_Payroll.DTOs.Overtimes;
using Overtime_Payroll.Models;
using Overtime_Payroll.Utilities;
using Overtime_Payroll.Utilities.Handlers;

namespace Overtime_Payroll.Repositories
{
    public class OvertimeRepository : GeneralRepository<Overtime>, IOvertimeRepository
    {
        public OvertimeRepository(OvertimeDbContext context) : base(context) { }

        public string? GetLastOvertimeId()
        {
            return _context.Set<Overtime>().ToList().Select(over => over.OvertimeId).LastOrDefault();
        }
  
        public Overtime? CreateOvertime(Overtime overtime)
        {
            try
            {
                var remainingOvertime = RemainingOvertimeByEmployeeGuid(overtime.EmployeeGuid);
                var exitingOvertime = _context.Overtimes.FirstOrDefault(o => o.StartOvertimeDate.Day == overtime.StartOvertimeDate.Day);
                if (remainingOvertime.OvertimeRemaining > 0 && remainingOvertime.EndOvertimeDate <= remainingOvertime.StartOvertimeDate)
                {
                    var emp = _context.Employees.Where(e => e.Guid == overtime.EmployeeGuid)
                        .Join(_context.Payrolls, e => e.Guid, p => p.EmployeeGuid, (e, p) => new
                        {
                            Employee = e,
                            Payroll = p,
                        }).FirstOrDefault();
                    var salaryPerHours = emp.Payroll.Salary * 1 / 173;
                    var totalHours = Convert.ToInt32((overtime.EndOvertimeDate - overtime.StartOvertimeDate).TotalHours);
                    var today = overtime.StartOvertimeDate.DayOfWeek;
                    if (today == DayOfWeek.Saturday || today == DayOfWeek.Sunday)
                    {
                        if (totalHours > 11)
                        {
                            totalHours = 11;
                        }
                        overtime.PaidOvertime = GetTotalPaidPerWeekend.PaidPerWeekend(totalHours, salaryPerHours);
                    }
                    else
                    {
                        if (totalHours > 4)
                        {
                            totalHours = 4;
                        }
                        for (int i = 0; i < totalHours; i++)
                        {
                            if (i < 1)
                            {
                                overtime.PaidOvertime += Convert.ToInt32(1.5 * salaryPerHours);
                            }
                            else
                            {
                                overtime.PaidOvertime += 2 * salaryPerHours;
                            }
                        }
                    }
                    //--------------------------------------
                    //update ditambahin + PPH 21 Nantinya
                    //--------------------------------------
                    overtime.OvertimeRemaining = remainingOvertime.OvertimeRemaining - totalHours;
                    _context.Set<Overtime>().Add(overtime);
                    _context.SaveChanges();
                    return overtime;
                }
                else return null;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public IEnumerable<AllRemainingOvertimeDto> RemainingOvertimeList()
        {
            var today = DateTime.Today;
            var targetDate = new DateTime(today.Year, 9, 25);
            var endDate = targetDate.AddDays(-30);
            var overtimeRemaining = (from c in _context.Overtimes
                           join emp in _context.Employees on c.EmployeeGuid equals emp.Guid
                           where (c.Status == Utilities.Enums.StatusLevel.Accepted && c.StartOvertimeDate >= endDate && c.EndOvertimeDate <= targetDate)
                           select new
                           {
                               guid = emp.Guid,
                               total = (c.EndOvertimeDate - c.StartOvertimeDate).TotalHours
                           }).ToList().GroupBy(a => a.guid).Select(b => new AllRemainingOvertimeDto()
                           {
                               EmployeeGuid = b.Key,
                               OvertimeRemaining = Convert.ToInt32(40 - b.Sum(c => c.total)),
                           }).ToList();
            return overtimeRemaining;
        }

        public IEnumerable<AllRemainingOvertimeDto> RemainingOvertimeList(Guid guid)
        {
            var today = DateTime.Today;
            var targetDate = new DateTime(today.Year, 9, 25);
            var endDate = targetDate.AddDays(-30);
            var overtimeRemaining = (from c in _context.Overtimes
                           join emp in _context.Employees on c.EmployeeGuid equals emp.Guid
                           where (c.Status == Utilities.Enums.StatusLevel.Accepted || c.Status == Utilities.Enums.StatusLevel.Waiting
                           && c.StartOvertimeDate >= endDate && c.EndOvertimeDate <= targetDate && emp.Guid == guid)
                           select new AllRemainingOvertimeDto()
                           {
                               Guid = c.Guid,
                               EmployeeGuid = emp.Guid,
                               EndOvertimeDate = c.EndOvertimeDate,
                               StartOvertimeDate = c.StartOvertimeDate,
                               FullName = emp.FirstName + " " + emp.LastName,
                               OvertimeId = c.OvertimeId,
                               Paid = c.PaidOvertime,
                               OvertimeRemaining = c.OvertimeRemaining,
                               Remarks = c.Remarks,
                               Status = c.Status,
                               CreatedDate = c.StartOvertimeDate
                           }).ToList();
            return overtimeRemaining;
        }

        public AllRemainingOvertimeDto RemainingOvertimeByEmployeeGuid(Guid guid)
        {

            var remaining = RemainingOvertimeList();
            var employeeRemainingOvertime = remaining.FirstOrDefault(a => a.EmployeeGuid == guid);
            if (employeeRemainingOvertime == null)
            {
                var employee = _context.Employees.FirstOrDefault(a => a.Guid == guid);
                var employeeRemain = new AllRemainingOvertimeDto();
                employeeRemain.EmployeeGuid = employee.Guid;
                employeeRemain.OvertimeRemaining = 40;
                return employeeRemain;
            }
            return employeeRemainingOvertime;

        }
        
        public IEnumerable<AllRemainingOvertimeDto> RemainingOvertimeByManagerGuidList(Guid managerGuid)
        {
            try
            {
                var overtime = (from m in _context.Employees
                              where m.ManagerGuid == managerGuid
                              join o in _context.Overtimes on m.Guid equals o.EmployeeGuid
                              select new AllRemainingOvertimeDto
                              {
                                  Guid = o.Guid,
                                  StartOvertimeDate = o.StartOvertimeDate,
                                  EndOvertimeDate = o.EndOvertimeDate,
                                  FullName = m.FirstName + " " + m.LastName,
                                  OvertimeId = o.OvertimeId,
                                  Paid = o.PaidOvertime,
                                  OvertimeRemaining = o.OvertimeRemaining,
                                  Remarks = o.Remarks,
                                  Status = o.Status,
                                  EmployeeGuid = o.EmployeeGuid,
                                  CreatedDate = o.StartOvertimeDate
                              }).Where(sta => sta.Status == Utilities.Enums.StatusLevel.Waiting
                              || sta.Status == Utilities.Enums.StatusLevel.Accepted || sta.Status == Utilities.Enums.StatusLevel.Rejected).ToList();

                return overtime;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}