
using Client.Contracts;
using Client.Controllers;
using Client.ViewModels.Overtimes;
using Newtonsoft.Json;
using server.Models;
using server.Utilities.Handlers;
using System.Net.Http;
using System.Text;

namespace Client.Repositories
{
    public class OvertimeRepository : GeneralRepository<RequestOvertimeViewModel, Guid>, IOvertimeRepository
    {
        public OvertimeRepository(string request = "overtimes/testing/") : base(request)
        {
        }

        public async Task<HandlerForResponse<IEnumerable<RequestOvertimeViewModel>>> GetOvertimeByManager(Guid guid)
        {

            HandlerForResponse<IEnumerable<RequestOvertimeViewModel>> entityVM = null;
            using (var response = await httpClient.GetAsync(request + "manager/" + guid))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<HandlerForResponse<IEnumerable<RequestOvertimeViewModel>>>(apiResponse);
            };
            return entityVM;
        }

        public async Task<HandlerForResponse<ApprovalByManager>> GetApproval(ApprovalByManager entity)
        {
            HandlerForResponse<ApprovalByManager> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            using (var response = httpClient.PutAsync(request + "update", content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<HandlerForResponse<ApprovalByManager>>(apiResponse);
            }
            return entityVM;
        }
    }
}