using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Enum;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dtos;
using Mango.Services.AuthAPI.Services.IService;
using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthAPI.Services
{
    public class AuthService(
        AppDbContext dbContext,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IJwtGeneratorService jwtGeneratorService
    ) : IAuthService
    {
        public async Task<bool> AssingRole(string? email, Role roleName)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            var user = dbContext.Users.FirstOrDefault(u => string.Equals(u.Email!.ToLower(), email!.ToLower()));

            if (user is null)
            {
                return false;
            }

            var role = await roleManager.RoleExistsAsync(roleName.ToString());

            if (!role)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName.ToString().ToUpper()));
            }

            await userManager.AddToRoleAsync(user, roleName.ToString());

            return true;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = dbContext.ApplicationUsers.FirstOrDefault(u => string.Equals(u.UserName!.ToLower(), loginRequestDto.UserName!.ToLower()));

            if (user is null)
            {
                return new LoginResponseDto
                {
                    Token = string.Empty,
                    User = null
                };
            }

            var isValid = await userManager.CheckPasswordAsync(user!, loginRequestDto.Password!);

            if (!isValid)
            {
                return new LoginResponseDto
                {
                    Token = string.Empty,
                    User = null
                };
            }

            var roles = await userManager.GetRolesAsync(user);

            return new LoginResponseDto
            {
                Token = jwtGeneratorService.GenerateToken(user, roles),
                User = new UserDto
                {
                    Email = user.Email,
                    Id = user.Id,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber
                }
            };
        }

        public async Task<string?> Register(RegistrationRequestDto registrationRequestDto)
        {
            var errorMessage = string.Empty;
            var appUser = new ApplicationUser
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                Name = registrationRequestDto.Name,
                PhoneNumber = registrationRequestDto.PhoneNumber
            };

            try
            {
                var result = await userManager.CreateAsync(appUser, registrationRequestDto.Password);

                if (!result.Succeeded)
                {
                    errorMessage = result.Errors.FirstOrDefault()?.Description;
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            return errorMessage;
        }
    }
}
