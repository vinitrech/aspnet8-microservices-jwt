using Mango.Services.AuthAPI.Models;

namespace Mango.Services.AuthAPI.Services.IService
{
    public interface IJwtGeneratorService
    {
        string GenerateToken(ApplicationUser applicationUser);

    }
}
