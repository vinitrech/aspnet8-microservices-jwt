namespace Mango.Services.AuthAPI.Models.Dtos
{
    public class ResponseDto
    {
        public object? Result { get; set; }
        public bool Success { get; set; } = false;
        public string? Message { get; set; } = string.Empty;
    }
}
