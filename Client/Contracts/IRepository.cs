using server.Utilities.Handlers;

namespace Client.Contracts
{
    public interface IRepository<T, X>
       where T : class
    {
        Task<HandlerForResponse<IEnumerable<T>>> Get();
        Task<HandlerForResponse<T>> Get(X id);
        Task<HandlerForResponse<T>> Post(T entity);
        Task<HandlerForResponse<T>> Put(X id, T entity);
        Task<HandlerForResponse<T>> Delete(X id);
    }
}
