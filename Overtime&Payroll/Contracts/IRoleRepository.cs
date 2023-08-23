using server.Models;

namespace server.Contracts
{
    public interface IRoleRepository : IGeneralRepository<Role>
    {
        Role? GetByName(string name);
    }
}
