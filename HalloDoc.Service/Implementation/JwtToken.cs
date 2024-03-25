using HalloDoc.DbEntity.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HalloDoc.Repositories.Interfaces;
using HalloDoc.Services.Interfaces;

namespace HalloDoc.Services
{
    public class JwtToken : IJwtToken
    {
        private readonly IConfiguration configuration;
        private readonly IAAdmin _admin;

        public JwtToken(IConfiguration configuration, IAAdmin admin)
        {
            this.configuration = configuration;
            this._admin = admin;
        }
        public string GenerateJwtToken(AspNetUser aspNetUser)
        {
            var roles = _admin.GetRole(aspNetUser.Id);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, aspNetUser.Email),
                new("Id", aspNetUser.Id.ToString()),
               
            };
            roles.ForEach(x => claims.Add(new Claim(ClaimTypes.Role, x)));


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("asldkfjasdlfkjas;dlkfjas;dlkfjasdl;fkjasdlfk;j"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);//HmacSha256 is a cryptographic hashing algorithm
            var expires = DateTime.UtcNow.AddMinutes(30);

            var token = new JwtSecurityToken(
                "Issuer",//issuer identifies the entity that issued the token
                "Audience",//the audience specifies who the token is intended for
                claims,
                expires: expires,
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public bool ValidateJwtToken(string token, out JwtSecurityToken jwtSecurityToken)
        {
            jwtSecurityToken = null;

            if (token == null)
            {
                return false;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("asldkfjasdlfkjas;dlkfjas;dlkfjasdl;fkjasdlfk;j");

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
        ,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                jwtSecurityToken = (JwtSecurityToken)validatedToken;

                if (jwtSecurityToken != null)
                {
                    return true;
                }
                return false;

            }
            catch
            {
                return false;
            }
        }

                
    }
}

