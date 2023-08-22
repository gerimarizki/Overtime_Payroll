using Overtime_Payroll.Models;

namespace Overtime_Payroll.Contracts
{
    public interface IAccountRepository : IGeneralRepository<Account>
    {
        bool IsDuplicateValue(string value);

        Account? GetEmployeeByEmail(string email);

    }

}