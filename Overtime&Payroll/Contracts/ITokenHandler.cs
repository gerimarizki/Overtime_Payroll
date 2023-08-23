using System.Security.Claims;

namespace server.Contracts
{
    public interface ITokenHandler
    {
        string GenerateToken(IEnumerable<Claim> claims);
    }

}
