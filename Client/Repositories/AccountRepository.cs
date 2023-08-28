using Client.Contracts;
using Newtonsoft.Json;
using server.DTOs.Accounts;
using server.Models;
using server.Utilities.Handlers;
using System.Text;

namespace Client.Repositories
{
    public class AccountRepository : GeneralRepository<Account, Guid>, IAccountRepository
    {
        private readonly HttpClient httpClient;
        private readonly string request;
        public AccountRepository(string request = "accounts/") : base(request)
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7128/api/")
            };
            this.request = request;
        }
        public async Task<HandlerForResponse<string>> Login(LoginAccountDto entity)
        {
            HandlerForResponse<string> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            using (var response = httpClient.PostAsync(request + "Login", content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<HandlerForResponse<string>>(apiResponse);
            }
            return entityVM;
        }
    }
}