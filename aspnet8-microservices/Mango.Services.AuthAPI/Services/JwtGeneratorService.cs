using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Configurations;
using Mango.Services.AuthAPI.Services.IService;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mango.Services.AuthAPI.Services
{
    public class JwtGeneratorService(JwtOptions jwtOptions) : IJwtGeneratorService
    {
        public string GenerateToken(ApplicationUser applicationUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.ASCII.GetBytes(jwtOptions.Secret);
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Email, applicationUser.Email!),
                new(JwtRegisteredClaimNames.Sub, applicationUser.Id!),
                new(JwtRegisteredClaimNames.Name, applicationUser.Name!)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = jwtOptions.Audience,
                Issuer = jwtOptions.Issuer,
                Subject = new(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
