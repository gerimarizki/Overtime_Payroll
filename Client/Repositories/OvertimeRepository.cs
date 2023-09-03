
using Client.Contracts;
using Client.ViewModels.Overtimes;
using Newtonsoft.Json;
using server.DTOs.Overtimes;
using server.Models;
using server.Utilities.Handlers;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Client.Repositories
{
    public class OvertimeRepository : GeneralRepository<Overtime, Guid>, IOvertimeRepository
    {

        private readonly string request;
        private readonly HttpClient httpClient;
        private readonly IHttpContextAccessor contextAccessor;
        public OvertimeRepository(string request = "overtimes/") : base(request)
        {
            this.request = request;
            contextAccessor = new HttpContextAccessor();
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7128/api/")
            };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", contextAccessor.HttpContext?.Session.GetString("JWToken"));
        }
        public async Task<HandlerForResponse<AllRemainingOvertimeDto>> PostOvertime(TestOvertimeDto entity)
        {
            HandlerForResponse<AllRemainingOvertimeDto> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            using (var response = httpClient.PostAsync(request + "create-overtime-to-employee-testing", content).Result) //ditambahkan 30/08/2023
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<HandlerForResponse<AllRemainingOvertimeDto>>(apiResponse);
            }
            return entityVM;
        }
    }
}