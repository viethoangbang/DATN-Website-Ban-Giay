using System.ComponentModel.DataAnnotations;

namespace Business.DTOs;

public class BrandBannerResponseDto
{
    public int Id { get; set; }
    public string Brand { get; set; } = null!;
    public int ImageId { get; set; }
    public string? ImageUrl { get; set; }
    public int? DisplayOrder { get; set; }
    public string? Status { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
}

public class BrandBannerCreateDto
{
    [Required(ErrorMessage = "Brand is required")]
    [MaxLength(100, ErrorMessage = "Brand name cannot exceed 100 characters")]
    public string Brand { get; set; } = null!;

    [Required(ErrorMessage = "Image ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid image ID")]
    public int ImageId { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Display order must be non-negative")]
    public int? DisplayOrder { get; set; }

    public string? Status { get; set; }
    public string? CreateBy { get; set; }
}

public class BrandBannerUpdateDto
{
    [MaxLength(100, ErrorMessage = "Brand name cannot exceed 100 characters")]
    public string? Brand { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Invalid image ID")]
    public int? ImageId { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Display order must be non-negative")]
    public int? DisplayOrder { get; set; }

    public string? Status { get; set; }
    public string? UpdateBy { get; set; }
}

public class BrandSectionDto
{
    public string Brand { get; set; } = null!;
    public List<BrandBannerResponseDto> Banners { get; set; } = new();
    public List<ProductResponseDto> Products { get; set; } = new();
}

