using server.DTOs.Accounts;
using server.Models;
using server.Utilities.Handlers;

namespace Client.Contracts
{
    public interface IAccountRepository : IRepository<Account, Guid>
    {
        public Task<HandlerForResponse<TokenDto>> Login(LoginAccountDto entity);
    }
}
