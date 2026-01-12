using System.ComponentModel.DataAnnotations;

namespace Business.DTOs;

// Image DTOs
public class ImageResponseDto
{
    public int Id { get; set; }
    public string? Url { get; set; }
    public string? Type { get; set; }
    public string? Status { get; set; }
}

public class ImageCreateDto
{
    [StringLength(1000, ErrorMessage = "URL không được vượt quá 1000 ký tự")]
    public string? Url { get; set; }
    
    public string? Type { get; set; }
    public string? Status { get; set; }
}

public class ImageUpdateDto
{
    [StringLength(1000, ErrorMessage = "URL không được vượt quá 1000 ký tự")]
    public string? Url { get; set; }
    
    public string? Type { get; set; }
    public string? Status { get; set; }
}
