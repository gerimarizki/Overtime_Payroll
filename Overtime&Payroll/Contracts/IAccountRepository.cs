using server.Models;

namespace server.Contracts
{
    public interface IAccountRepository : IGeneralRepository<Account>
    {
        bool IsDuplicateValue(string value);

        Account? GetEmployeeByEmail(string email);

    }

}