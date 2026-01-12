using System.ComponentModel.DataAnnotations;

namespace Business.DTOs;

public class AccountResponseDto
{
    public int Id { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FullName { get; set; }
    public string? Sex { get; set; }
    public int? BirthYear { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Status { get; set; }
    public List<string> Roles { get; set; } = new();
    public DateTime? CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
}

public class AccountCreateDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public string Password { get; set; } = null!;

    [Phone(ErrorMessage = "Invalid phone number")]
    public string? PhoneNumber { get; set; }

    [MaxLength(255)]
    public string? FullName { get; set; }

    [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Sex must be Male, Female, or Other")]
    public string? Sex { get; set; }

    [Range(1900, 2100, ErrorMessage = "Invalid birth year")]
    public int? BirthYear { get; set; }

    [Url(ErrorMessage = "Invalid URL format")]
    [MaxLength(1000)]
    public string? AvatarUrl { get; set; }

    [Required(ErrorMessage = "At least one role is required")]
    [MinLength(1, ErrorMessage = "At least one role is required")]
    public List<string> Roles { get; set; } = new() { "customer" };

    public string? Status { get; set; } = "Active";
}

public class AccountUpdateDto
{
    [Phone(ErrorMessage = "Invalid phone number")]
    public string? PhoneNumber { get; set; }

    [MaxLength(255)]
    public string? FullName { get; set; }

    [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Sex must be Male, Female, or Other")]
    public string? Sex { get; set; }

    [Range(1900, 2100, ErrorMessage = "Invalid birth year")]
    public int? BirthYear { get; set; }

    [Url(ErrorMessage = "Invalid URL format")]
    [MaxLength(1000)]
    public string? AvatarUrl { get; set; }

    public List<string>? Roles { get; set; }

    public string? Status { get; set; }
}

public class AccountChangePasswordDto
{
    [Required(ErrorMessage = "Current password is required")]
    public string CurrentPassword { get; set; } = null!;

    [Required(ErrorMessage = "New password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).*$", 
        ErrorMessage = "Password must contain uppercase, lowercase, number and special character")]
    public string NewPassword { get; set; } = null!;
}

