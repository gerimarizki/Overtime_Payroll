
using Client.Contracts;
using server.Models;

namespace Client.Repositories
{
    public class EmployeeRepository : GeneralRepository<Employee, Guid>, IEmployeeRepository
    {
        public EmployeeRepository(string request = "employees/") : base(request)
        {
        }
    }
}