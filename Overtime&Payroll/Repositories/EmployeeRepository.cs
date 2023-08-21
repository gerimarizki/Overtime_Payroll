using Overtime_Payroll.Contracts;
using Overtime_Payroll.Data;
using Overtime_Payroll.Models;

namespace Overtime_Payroll.Repositories
{
    public class EmployeeRepository : GeneralRepository<Employee>, IEmployeeRepository
    {

        public EmployeeRepository(OvertimeDbContext context) : base(context) { }

        public Employee? GetManagerByGuid(Guid? guid)
        {
            var employee = _context.Set<Employee>().Find(guid);

            var manager = _context.Set<Employee>().Find(employee.ManagerGuid);

            _context.ChangeTracker.Clear();
            return manager;
        }

        public string? GetLastEmployeeNik()
        {
            return _context.Set<Employee>().ToList().Select(e => e.NIK).LastOrDefault();
        }

        public bool IsDuplicateValue(string value)
        {
            return _context.Set<Employee>()
                           .FirstOrDefault(e => e.PhoneNumber.Contains(value)) is null;
        }

        public string? GetLastNik()
        {
            return _context.Set<Employee>().ToList().OrderBy(emp => emp.NIK).LastOrDefault()?.NIK;
        }

        public bool IsNotExist(string value)
        {
            return _context.Set<Employee>()
                           .SingleOrDefault(a => a.PhoneNumber.Contains(value)) is null;
        }
    }
}