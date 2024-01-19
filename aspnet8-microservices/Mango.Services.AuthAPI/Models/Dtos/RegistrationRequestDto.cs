using Mango.Services.AuthAPI.Enum;

namespace Mango.Services.AuthAPI.Models.Dtos
{
    public class RegistrationRequestDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public Role Role { get; set; }
    }
}
