using server.Utilities.Handlers;

namespace Client.Contracts
{
    public interface IRepository<TEntity, TId>
       where TEntity : class
    {
        Task<HandlerForResponse<IEnumerable<TEntity>>> Get();
        Task<HandlerForResponse<TEntity>> Get(TId id);
        Task<HandlerForResponse<TEntity>> Post(TEntity entity);
        Task<HandlerForResponse<TEntity>> Put(TId id, TEntity entity);
        Task<HandlerForResponse<TEntity>> Delete(TId id);
    }
}
