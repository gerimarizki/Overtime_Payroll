
using Client.Contracts;
using Client.ViewModels.Payroll;
using Newtonsoft.Json;
using server.Models;
using server.Repositories;
using server.Utilities.Handlers;
using System.Net.Http;

namespace Client.Repositories
{
    public class PayrollRepository : GeneralRepository<Payroll, Guid>, IPayrollRepository
    {
        public PayrollRepository(string request = "payrolls/") : base(request)
        {
            
        }
        //public async Task<HandlerForResponse<double>> GetStatistic()
        //{
        //    HandlerForResponse<double> entity = null;
        //    using (var response = await httpClient.GetAsync(request + "/total-expense"))
        //    {
        //        string apiResponse = await response.Content.ReadAsStringAsync();
        //        entity = JsonConvert.DeserializeObject<HandlerForResponse<double>>(apiResponse);
        //    }
        //    return entity;
        //}

      
    }
}