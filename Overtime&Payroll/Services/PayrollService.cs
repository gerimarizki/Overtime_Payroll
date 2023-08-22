using Overtime_Payroll.Contracts;
using Overtime_Payroll.DTOs.Payrolls;
using Overtime_Payroll.Models;

namespace Overtime_Payroll.Services
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
                               PayDate = DateTime.Now,
                               EmployeeGuid = e.Guid,
                               Allowace = p.Allowance,
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
                               PaidOvertime = b.Sum(c => c.totalOver)
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
                            join pay in Payrolls on p.EmployeeGuid equals pay.EmployeeGuid
                            select new GetAllPayrollDto
                            {
                                Guid = p.Guid,
                                Salary = p.Salary,
                                Allowance = p.Allowance,
                                PayDate = DateTime.Now,
                                PaidOvertime = pay.PaidOvertime,
                                TotalSalary = p.Salary + pay.TotalSalary - p.Allowance,
                                EmployeeGuid = pay.EmployeeGuid,
                                FullName = e.FirstName + " " + e.LastName,
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
                                TotalSalary = p.Salary + pay.TotalSalary - p.Allowance,
                                EmployeeGuid = pay.EmployeeGuid,
                            }).ToList();
            return employee;
        }

        public IEnumerable<GetAllPayrollDto> GetAllPayrollOverbyEmpGuid(Guid guid)
        {
            var Payroll = GetAllOverEmpGuid(guid);
            var employee = (from p in _PayrollRepository.GetAll()
                            join pay in Payroll on p.EmployeeGuid equals pay.EmployeeGuid
                            select new GetAllPayrollDto
                            {
                                Guid = p.Guid,
                                Salary = p.Salary,
                                Allowance = p.Allowance,
                                PayDate = pay.PayDate,
                                PaidOvertime = pay.PaidOvertime,
                                TotalSalary = p.Salary + pay.TotalSalary - p.Allowance,
                                EmployeeGuid = pay.EmployeeGuid,
                                FullName= pay.FullName


                            }).ToList();
            return employee;
        }

        public double GetTotalSalaryExpense()
        {
            var Payrolls = _PayrollRepository.GetAll();

            // Calculate total salary expense by summing up all salaries
            double totalExpense = Payrolls.Sum(p => p.Salary);

            return totalExpense;
        }
        public double GetTotalOvertime(Guid guid)
        {
            var Payrolls = GetAllPayrollOverbyEmpGuid(guid);
            double totalOvertime = Payrolls.Sum(p => p.PaidOvertime);
            return totalOvertime;
        }
    }
}
