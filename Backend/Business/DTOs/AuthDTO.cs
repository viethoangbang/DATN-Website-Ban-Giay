using System.ComponentModel.DataAnnotations;

namespace Business.DTOs;

// Auth DTOs with validation
public class RegisterDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).*$", 
        ErrorMessage = "Password must contain uppercase, lowercase, number and special character")]
    public string Password { get; set; } = null!;

    [Phone(ErrorMessage = "Invalid phone number")]
    public string? PhoneNumber { get; set; }

    [MaxLength(255)]
    public string? FullName { get; set; }

    [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Sex must be Male, Female, or Other")]
    public string? Sex { get; set; }

    [Range(1900, 2100, ErrorMessage = "Invalid birth year")]
    public int? BirthYear { get; set; }

    [Required(ErrorMessage = "Role is required")]
    [RegularExpression("^(admin|employee|customer)$", ErrorMessage = "Role must be admin, employee, or customer")]
    public string Role { get; set; } = "customer";
}

public class LoginDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = null!;
}

public class LoginResponseDto
{
    public string Token { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<string> Roles { get; set; } = new();
    public int AccountId { get; set; }
    public string? FullName { get; set; }
}
