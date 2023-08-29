using Client.Contracts;
using Newtonsoft.Json;
using server.Utilities.Handlers;
using System.Net.Http.Headers;
using System.Text;

namespace Client.Repositories
{
    public class GeneralRepository<TEntity, TId> : IRepository<TEntity, TId>
       where TEntity : class
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

        public async Task<HandlerForResponse<TEntity>> Delete(TId id)
        {
            HandlerForResponse<TEntity> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");

            using (var response = httpClient.DeleteAsync(request + "?guid=" + id).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<HandlerForResponse<TEntity>>(apiResponse);
            }
            return entityVM;
        }

        public async Task<HandlerForResponse<IEnumerable<TEntity>>> Get()
        {
            HandlerForResponse<IEnumerable<TEntity>> entityVM = null;
            using (var response = await httpClient.GetAsync(request))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<HandlerForResponse<IEnumerable<TEntity>>>(apiResponse);
            }
            return entityVM;
        }

        public async Task<HandlerForResponse<TEntity>> Get(TId id)
        {
            HandlerForResponse<TEntity> entity = null;

            using (var response = await httpClient.GetAsync(request + id))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entity = JsonConvert.DeserializeObject<HandlerForResponse<TEntity>>(apiResponse);
            }
            return entity;
        }

        public async Task<HandlerForResponse<TEntity>> Post(TEntity entity)
        {
            HandlerForResponse<TEntity> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            using (var response = httpClient.PostAsync(request, content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<HandlerForResponse<TEntity>>(apiResponse);
            }
            return entityVM;
        }

        public async Task<HandlerForResponse<TEntity>> Put(TId id, TEntity entity)
        {
            HandlerForResponse<TEntity> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            using (var response = httpClient.PutAsync(request, content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<HandlerForResponse<TEntity>>(apiResponse);
            }
            return entityVM;
        }
    }
}
