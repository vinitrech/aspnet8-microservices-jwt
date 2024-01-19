using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Configurations;
using Mango.Services.AuthAPI.Services.IService;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mango.Services.AuthAPI.Services
{
    public class JwtGeneratorService(IOptions<JwtOptions> jwtOptions) : IJwtGeneratorService
    {
        public string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.ASCII.GetBytes(jwtOptions.Value.Secret);
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Email, applicationUser.Email!),
                new(JwtRegisteredClaimNames.Sub, applicationUser.Id!),
                new(JwtRegisteredClaimNames.Name, applicationUser.Name!)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role))); // When [Authorize(Role="Admin")] is used, for example, aspnetcore identity will look for the claim of type Role with value "Admin" to validate the permission in the token

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = jwtOptions.Value.Audience,
                Issuer = jwtOptions.Value.Issuer,
                Subject = new(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
