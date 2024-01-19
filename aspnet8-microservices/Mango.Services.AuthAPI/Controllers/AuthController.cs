using Mango.Services.AuthAPI.Constants;
using Mango.Services.AuthAPI.Models.Dtos;
using Mango.Services.AuthAPI.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly ResponseDto responseDto = new();

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            var errorMessage = await authService.Register(registrationRequestDto);

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                responseDto.Success = false;
                responseDto.Message = errorMessage;

                return BadRequest(responseDto);
            }

            return Ok(responseDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var loginResponseDto = await authService.Login(loginRequestDto);

            if (loginResponseDto?.User is null)
            {
                responseDto.Success = false;
                responseDto.Message = "Invalid username/password";

                return BadRequest(responseDto);
            }

            responseDto.Result = loginResponseDto;

            return Ok(responseDto);
        }

        [HttpPost("assign-role")]
        [Authorize(Policy = RolePoliciesConstants.RequireAdminRole)]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            var assignRoleSuccessful = await authService.AssingRole(registrationRequestDto.Email, registrationRequestDto.Role);

            if (!assignRoleSuccessful)
            {
                responseDto.Success = false;
                responseDto.Message = $"Error while assigning user {registrationRequestDto.Email} to role {registrationRequestDto.Role}";

                return BadRequest(responseDto);
            }

            return Ok(responseDto);
        }
    }
}
