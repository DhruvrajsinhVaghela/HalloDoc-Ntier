using System.IdentityModel.Tokens.Jwt;
using HalloDoc.DbEntity.Models;

namespace HalloDoc.Services.Interfaces
{
    public interface IJwtToken
    {
        string GenerateJwtToken(AspNetUser Asp);

        bool ValidateJwtToken(string token, out JwtSecurityToken jwtSecurityToken);
        
    }
}

