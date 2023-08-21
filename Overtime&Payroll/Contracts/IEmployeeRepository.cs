using Overtime_Payroll.Models;

namespace Overtime_Payroll.Contracts
{
    public interface IEmployeeRepository : IGeneralRepository<Employee>
    {
        //public string? GetLastEmployeeNik();

        bool IsDuplicateValue(string value);

        public Employee? GetManagerByGuid(Guid? guid);
        string? GetLastNik();

        public bool IsNotExist(string value);
    }
}
