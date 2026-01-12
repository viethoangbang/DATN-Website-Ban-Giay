namespace Business.DTOs;

public class RegisterResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
    public string? Token { get; set; }
    public UserInfo? User { get; set; }
}
