using server.Contracts;
using server.DTOs.Payrolls;
using server.Models;
using server.Repositories;
using server.Utilities.Enums;
using System.ComponentModel.DataAnnotations;

namespace server.Services
{
    public class PayrollService
    {
        private readonly IPayrollRepository _PayrollRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IOvertimeRepository _overtimeRepository;

        public PayrollService(IPayrollRepository PayrollRepository, IEmployeeRepository employeeRepository, IOvertimeRepository overtimeRepository)
        {
            _PayrollRepository = PayrollRepository;
            _employeeRepository = employeeRepository;
            _overtimeRepository = overtimeRepository;
        }

        public IEnumerable<GetPayrollDto> GetPayroll()
        {
            var payroll = (from e in _employeeRepository.GetAll()
                           join p in _PayrollRepository.GetAll() on e.Guid equals p.EmployeeGuid
                           where e.Guid == p.EmployeeGuid
                           select new GetPayrollDto
                           {
                               Guid = p.Guid,
                               PayDate = new DateTime(2023, 8, 25),
                               EmployeeGuid = e.Guid,
                               Allowance = p.Allowance,
                               Salary = p.Salary,
                               EmployeeName = e.FirstName + " " + e.LastName
                           }).ToList();
            if (!payroll.Any())
            {
                return Enumerable.Empty<GetPayrollDto>();
            }
            return payroll;
        }

        public GetPayrollDto? GetPayrollDtoByGuid(Guid guid)
        {
            var Payroll = _PayrollRepository.GetByGuid(guid);
            if (Payroll == null)
            {
                return null;
            }

            return (GetPayrollDto)Payroll;
        }

        public GetPayrollDto? CreatePayroll(CreatePayrollDto newPayroll)
        {
            Payroll Payrolls = newPayroll;
            var Payroll = _PayrollRepository.Create(Payrolls);
            if (Payroll == null) return null;

            return (GetPayrollDto)Payroll;
        }

        public int UpdatePayrolls(UpdatePayrollDto updatePayroll)
        {
            var Payrollps = _PayrollRepository.GetByGuid(updatePayroll.Guid);
            if (Payrollps == null)
            {
                return -1;
            }
            var isUpdate = _PayrollRepository.Update(updatePayroll);
            return !isUpdate ? 0 : 1;
        }

        public int DeletePayroll(Guid guid)
        {
            var Payroll = _PayrollRepository.GetByGuid(guid);
            if (Payroll == null) { return -1; }

            var isDelete = _PayrollRepository.Delete(Payroll);
            return !isDelete ? 0 : 1;
        }

        public IEnumerable<GetAllPayrollDto> GetAllOvertime()
        {
            var Payroll = (from p in _PayrollRepository.GetAll()
                           join o in _overtimeRepository.GetAll() on p.EmployeeGuid equals o.EmployeeGuid
                           select new
                           {
                               guid = o.EmployeeGuid,
                               totalOver = o.PaidOvertime,
                           }).ToList().GroupBy(a => a.guid).Select(b => new GetAllPayrollDto()
                           {
                               EmployeeGuid = b.Key,
                               PaidOvertime = b.Sum(c => c.totalOver)
                           }).ToList();
            return Payroll;
        }

        public IEnumerable<GetAllPayrollDto> GetAllPayroll()
        {
            var Payroll = (from p in _PayrollRepository.GetAll()
                           join o in _overtimeRepository.GetAll() on p.EmployeeGuid equals o.EmployeeGuid into joined
                           from o in joined.DefaultIfEmpty()
                           where o == null // This line will filter out the rows where there is no matching EmployeeGuid in the overtime table.
                           select new
                           {
                               guid = p.EmployeeGuid,
                               totalOver = 0 // Set the PaidOvertime to 0 since there is no matching overtime record.
                           }).ToList().GroupBy(a => a.guid).Select(b => new GetAllPayrollDto()
                           {
                               EmployeeGuid = b.Key,
                               PaidOvertime = b.Sum(c => c.totalOver)
                           }).ToList();

            return Payroll;
        }

        public IEnumerable<GetAllPayrollDto> GetAllOvertime(Guid guid)
        {
            var Payroll = (from p in _PayrollRepository.GetAll()
                           join o in _overtimeRepository.GetAll() on p.EmployeeGuid equals o.EmployeeGuid
                           where p.Guid == guid
                           select new
                           {

                               guid = o.EmployeeGuid,
                               totalOver = o.PaidOvertime,
                           }).ToList().GroupBy(a => a.guid).Select(b => new GetAllPayrollDto()
                           {
                               EmployeeGuid = b.Key,
                               PaidOvertime = b.Sum(c => c.totalOver),
                    
                               
                           }).ToList();
            return Payroll;
        }
        public IEnumerable<GetAllPayrollDto> GetAllOverEmpGuid(Guid guid)
        {
            var Payroll = (from p in _PayrollRepository.GetAll()
                           join o in _overtimeRepository.GetAll() on p.EmployeeGuid equals o.EmployeeGuid
                           where p.EmployeeGuid == guid
                           select new
                           {
                               guid = o.EmployeeGuid,
                               totalOver = o.PaidOvertime,
                           }).ToList().GroupBy(a => a.guid).Select(b => new GetAllPayrollDto()
                           {
                               EmployeeGuid = b.Key,
                               PaidOvertime = b.Sum(c => c.totalOver)
                           }).ToList();
            return Payroll;
        }

        public IEnumerable<GetAllPayrollDto> GetAllMasterOver()
        {
            var Payroll = GetAllOvertime();
            var Payrolls = new List<GetAllPayrollDto>();
            Payrolls = Payroll.ToList();

            var noOvertime = GetAllPayroll();
            var noOverTimes = new List<GetAllPayrollDto>();
            noOverTimes = noOvertime.ToList();

            Payrolls.AddRange(noOverTimes);

            var employee = (from p in _PayrollRepository.GetAll()
                            join e in _employeeRepository.GetAll() on p.EmployeeGuid equals e.Guid
                            join o in _overtimeRepository.GetAll() on e.Guid equals o.EmployeeGuid
                            //join pay in Payrolls on p.EmployeeGuid equals pay.EmployeeGuid
                            select new GetAllPayrollDto
                            {
                                Guid = p.Guid,
                                FullName = e.FirstName + " " + e.LastName,
                                Salary = p.Salary,
                                Allowance = p.Allowance,
                                PayDate = new DateTime(2023, 8, 25),
                                PaidOvertime = o.PaidOvertime,
                                TotalSalary = p.Salary + o.PaidOvertime - p.Allowance,
                                EmployeeGuid = e.Guid,
                                OvertimeRemaining= o.OvertimeRemaining.ToString()
                            }).ToList();
            return employee;
        }

        public IEnumerable<GetAllPayrollDto> GetAllMasterOverbyGuid(Guid guid)
        {
            var Payroll = GetAllOvertime(guid);
            var employee = (from p in _PayrollRepository.GetAll()
                            join pay in Payroll on p.EmployeeGuid equals pay.EmployeeGuid
                            select new GetAllPayrollDto
                            {
                                Guid = p.Guid,
                                Salary = p.Salary,
                                Allowance = p.Allowance,
                                PayDate = p.PayDate,
                                PaidOvertime = pay.PaidOvertime,
                                TotalSalary = p.Salary + pay.PaidOvertime - p.Allowance,
                                EmployeeGuid = pay.EmployeeGuid,
                                OvertimeRemaining = pay.OvertimeRemaining.ToString()
                            }).ToList();
            return employee;
        }

        public IEnumerable<GetAllPayrollDto> GetAllPayrollOverbyEmpGuid(Guid guid)
        {
            var Payroll = GetAllOverEmpGuid(guid);
            var employee = (from p in _PayrollRepository.GetAll()
                            join e in _employeeRepository.GetAll() on p.EmployeeGuid equals e.Guid
                            join o in _overtimeRepository.GetAll() on e.Guid equals o.EmployeeGuid
                            join pay in Payroll on p.EmployeeGuid equals pay.EmployeeGuid
                            select new GetAllPayrollDto
                            {
                                Guid = o.Guid,                                
                                Salary = p.Salary,
                                Allowance = p.Allowance,
                                PayDate = new DateTime(25/08/2023),
                                PaidOvertime = pay.PaidOvertime,
                                TotalSalary = p.Salary + pay.PaidOvertime - p.Allowance,
                                EmployeeGuid = pay.EmployeeGuid,
                                FullName = e.FirstName + " " + e.LastName,
                                OvertimeRemaining = o.OvertimeRemaining.ToString()
                                //TotalPaidOvertime = GetTotalOvertime(pay.EmployeeGuid) // Menghitung total paid overtime
                            }).ToList();
            return employee;

        }



        public double GetAllTotalSalary()
        {
            var Payrolls = _PayrollRepository.GetAll();


            double totalExpense = Payrolls.Sum(p => p.Salary - p.Allowance);

            return totalExpense;
        }

        public double GetTotalSalary(Guid employeeGuid)
        {
            var payrolls = GetAllPayrollOverbyEmpGuid(employeeGuid);
            var paidOvertime = GetAllMasterOverbyGuid(employeeGuid);

            double totalSalary = payrolls.Sum(p => p.Salary + p.PaidOvertime - p.Allowance);

            return totalSalary;
        }


        public double GetTotalOvertime(Guid guid)
        {
            var Payrolls = GetAllPayrollOverbyEmpGuid(guid);
            double totalOvertime = Payrolls.Sum(p => p.PaidOvertime);
            return totalOvertime;
        }


        public GetPayslipDto? GetPayslip(Guid guid)
        {
            var payover = GetAllPayrollOverbyEmpGuid(guid);
            var payslip = (from p in _PayrollRepository.GetAll()
                           join e in _employeeRepository.GetAll() on p.EmployeeGuid equals e.Guid
                           join o in _overtimeRepository.GetAll() on e.Guid equals o.EmployeeGuid
                           join pay in payover on p.EmployeeGuid equals pay.EmployeeGuid
                           select new GetPayslipDto
                           {
                               FullName = e.FirstName + " " + e.LastName,
                               Allowance = p.Allowance,
                               PayDate = new DateTime(2023, 8, 25),
                               Salary = pay.Salary,
                               PaidOvertime = pay.PaidOvertime,
                               TotalSalary = pay.Salary - p.Allowance,
                               Status = o.Status,
                           });

            var lastPaid = payslip.LastOrDefault();

            // Cek status Paid Overtime
            if (lastPaid != null && lastPaid.Status == StatusLevel.Waiting)
            {
                // Jika status "Waiting", maka Paid Overtime tetap 0
                lastPaid.PaidOvertime = 0;
            }
            else if (lastPaid != null && lastPaid.Status == StatusLevel.Accepted)
            {
                // Jika status "Accepted", maka ambil nilai Paid Overtime dari data
                lastPaid.PaidOvertime = payover.Last().PaidOvertime;
            }

            // Hitung Total Salary
            lastPaid.TotalSalary = lastPaid.Salary + lastPaid.PaidOvertime - lastPaid.Allowance;

            return lastPaid;
        }
        public IEnumerable<GetPayslipDto>? GetAllDetail(Guid guid)
        {
            var payslip = from payroll in _PayrollRepository.GetAll()
                          join employee in _employeeRepository.GetAll() on payroll.EmployeeGuid equals employee.Guid
                          join overtime in _overtimeRepository.GetAll() on employee.Guid equals overtime.EmployeeGuid
                          where employee.Guid == guid
                          select new GetPayslipDto
                          {
                              EmployeeGuid = employee.Guid,
                              OvertimeId = overtime.OvertimeId,
                              FullName = employee.FirstName + " " + employee.LastName,
                              Allowance = payroll.Allowance,
                              PayDate = new DateTime(2023, 8, 25),
                              Salary = payroll.Salary,
                              PaidOvertime = overtime.PaidOvertime,
                              TotalSalary = payroll.Salary + overtime.PaidOvertime - payroll.Allowance,
                              Status = overtime.Status,
                              Counter = 0
                          };
            return payslip;
        }
        public GetPayslipDto? GetDetail(Guid guid)
        {
            var payslip = from payroll in _PayrollRepository.GetAll()
                          join employee in _employeeRepository.GetAll() on payroll.EmployeeGuid equals employee.Guid
                          join overtime in _overtimeRepository.GetAll() on employee.Guid equals overtime.EmployeeGuid
                          where employee.Guid == guid
                          select new GetPayslipDto
                          {
                              EmployeeGuid = employee.Guid,
                              OvertimeId = overtime.OvertimeId,
                              FullName = employee.FirstName + " " + employee.LastName,
                              Allowance = payroll.Allowance,
                              PayDate = new DateTime(2023, 8, 25),
                              Salary = payroll.Salary,
                              PaidOvertime = overtime.PaidOvertime,
                              TotalSalary = payroll.Salary + overtime.PaidOvertime - payroll.Allowance,
                              Status = overtime.Status,
                              CreatedDate = overtime.CreatedDate,
                              Counter = 0
                          };
            /*var lastPaid = payslip.Where(lp=>lp.EmployeeGuid.Equals(guid));*/
            /*var lastovertime = _overtimeRepository.GetOvertimeByOvertimeId(payslip.LastOrDefault().OvertimeId);*/
            var newovertime = new GetPayslipDto();
            newovertime = payslip.LastOrDefault();
            var counterpayslip = payslip.Count();
            newovertime.Counter = counterpayslip;
            
            var newpayslip = payslip.OrderByDescending(payslip => payslip.PaidOvertime).FirstOrDefault();
            newpayslip.PaidOvertime = 0;
            foreach(var item1 in payslip)
            {
                if(newovertime.Counter>1)
                {
                    newpayslip.PaidOvertime += item1.PaidOvertime;
                }
                newovertime.Counter--;
            }/*
            newovertime.Counter = 1;
            foreach (var item1 in payslip)
            {
                if(newovertime.Counter < payslip.Count())
                {
                    newpayslip.PaidOvertime += item1.PaidOvertime;
                }
                newovertime.Counter++;
            }*/
            newovertime.Counter = counterpayslip;
            foreach (var item in payslip)
            {
                if (newovertime.Status == StatusLevel.Waiting)
                {
                    newovertime = newpayslip;
                    break;
                }
                else if(newovertime.Status == StatusLevel.Accepted && newovertime.Counter > 1)
                {

                    newovertime.PaidOvertime += item.PaidOvertime;
                }
                newovertime.Counter--;
            }

            newovertime.TotalSalary = newovertime.Salary + newovertime.PaidOvertime - newovertime.Allowance;
            return newovertime;
        }
       /* public IEnumerable<GetPayslipDto>? GetDetailbyGuid(Guid id)
        {
            return GetDetail().Where(pay => pay.EmployeeGuid.Equals(id));
        }*/

    }
}
