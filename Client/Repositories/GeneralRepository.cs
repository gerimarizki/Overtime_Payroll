using Client.Contracts;
using Newtonsoft.Json;
using server.Utilities.Handlers;
using System.Net.Http.Headers;
using System.Text;

namespace Client.Repositories
{
    public class GeneralRepository<Entity, TId> : IRepository<Entity, TId>
       where Entity : class
    {
        protected readonly string request;
        protected readonly HttpClient httpClient;
        protected readonly IHttpContextAccessor contextAccessor;

        public GeneralRepository(string request)
        {
            this.request = request;
            contextAccessor = new HttpContextAccessor();
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7128/api/")
            };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", contextAccessor.HttpContext?.Session.GetString("JWToken"));
        }

        public async Task<HandlerForResponse<Entity>> Delete(TId id)
        {
            HandlerForResponse<Entity> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");

            using (var response = httpClient.DeleteAsync(request + "?guid=" + id).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<HandlerForResponse<Entity>>(apiResponse);
            }
            return entityVM;
        }

        public async Task<HandlerForResponse<IEnumerable<Entity>>> Get()
        {
            HandlerForResponse<IEnumerable<Entity>> entityVM = null;
            using (var response = await httpClient.GetAsync(request))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<HandlerForResponse<IEnumerable<Entity>>>(apiResponse);
            }
            return entityVM;
        }

        public async Task<HandlerForResponse<Entity>> Get(TId id)
        {
            HandlerForResponse<Entity> entity = null;

            using (var response = await httpClient.GetAsync(request + id))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entity = JsonConvert.DeserializeObject<HandlerForResponse<Entity>>(apiResponse);
            }
            return entity;
        }

        public async Task<HandlerForResponse<Entity>> Post(Entity entity)
        {
            HandlerForResponse<Entity> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            using (var response = httpClient.PostAsync(request, content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<HandlerForResponse<Entity>>(apiResponse);
            }
            return entityVM;
        }

        public async Task<HandlerForResponse<Entity>> Put(TId id, Entity entity)
        {
            HandlerForResponse<Entity> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            using (var response = httpClient.PutAsync(request, content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<HandlerForResponse<Entity>>(apiResponse);
            }
            return entityVM;
        }
    }
}
