namespace Mango.Services.AuthAPI.Models.Configurations
{
    public class JwtOptions
    {
        public string Secret { get; } = string.Empty;
        public string Issuer { get; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
    }
}
