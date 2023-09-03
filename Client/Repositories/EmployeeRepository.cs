
using Client.Contracts;
using Newtonsoft.Json;
using server.Models;
using server.Utilities.Handlers;
using System.Net.Http;

namespace Client.Repositories
{
    public class EmployeeRepository : GeneralRepository<Employee, Guid>, IEmployeeRepository
    {
        public EmployeeRepository(string request = "employees/") : base(request)
        {
        }
        public async Task<HandlerForResponse<int>> GetCount()
        {
            HandlerForResponse<int> entity = null;
            using (var response = await httpClient.GetAsync(request + "total-count"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entity = JsonConvert.DeserializeObject<HandlerForResponse<int>>(apiResponse);
            }
            return entity;
        }
    }
}
