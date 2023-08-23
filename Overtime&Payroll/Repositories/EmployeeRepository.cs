using server.Contracts;
using server.Data;
using server.Models;

namespace server.Repositories
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

        public string? GetLastEmployeeNIK()
        {
            return _context.Set<Employee>().ToList().Select(e => e.NIK).LastOrDefault();
        }

        public bool IsDuplicateValue(string value)
        {
            return _context.Set<Employee>()
                           .FirstOrDefault(e => e.PhoneNumber.Contains(value)) is null;
        }
    }
}