using System.Security.Claims;

namespace Overtime_Payroll.Contracts
{
    public interface ITokenHandler
    {
        string GenerateToken(IEnumerable<Claim> claims);
    }

}
