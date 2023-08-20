using Microsoft.IdentityModel.Tokens;
using Overtime_Payroll.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Overtime_Payroll.Utilities.Handlers
{
    public class HandlerForToken : ITokenHandler
    {
        private readonly IConfiguration _configuration;

        public HandlerForToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTService:Key"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(issuer: _configuration["JWTService:Issuer"],
                                                    audience: _configuration["JWTService:Audience"],
                                                    claims: claims,
                                                    expires: DateTime.Now.AddMinutes(60),
                                                    signingCredentials: signinCredentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return tokenString;
        }
    }
}
