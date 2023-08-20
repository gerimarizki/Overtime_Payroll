using Overtime_Payroll.Models;

namespace Overtime_Payroll.Contracts
{
    public interface IEmployeeRepository : IGeneralRepository<Employee>
    {
        public string? GetLastEmployeeNIK();

        bool IsDuplicateValue(string value);

        public Employee? GetManagerByGuid(Guid? guid);
    }
}
