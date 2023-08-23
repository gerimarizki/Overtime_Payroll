using server.Models;

namespace server.Contracts
{
    public interface IEmployeeRepository : IGeneralRepository<Employee>
    {
        public string? GetLastEmployeeNIK();

        bool IsDuplicateValue(string value);

        public Employee? GetManagerByGuid(Guid? guid);
    }
}
