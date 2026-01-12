using System.ComponentModel.DataAnnotations;

namespace Business.DTOs;

public class DiscountResponseDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public string DiscountType { get; set; } = null!; // "Percentage" or "Fixed"
    public decimal DiscountValue { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Status { get; set; }
    public string? CreateBy { get; set; }
    public DateTime? CreateDate { get; set; }
    public string? UpdateBy { get; set; }
    public DateTime? UpdateDate { get; set; }
}

public class DiscountCreateDto
{
    [Required(ErrorMessage = "Product ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid product ID")]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Discount type is required")]
    [RegularExpression("^(Percentage|Fixed)$", ErrorMessage = "Discount type must be 'Percentage' or 'Fixed'")]
    public string DiscountType { get; set; } = null!;

    [Required(ErrorMessage = "Discount value is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Discount value must be non-negative")]
    public decimal DiscountValue { get; set; }

    [Required(ErrorMessage = "Start date is required")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "End date is required")]
    public DateTime EndDate { get; set; }

    [MaxLength(50)]
    public string? Status { get; set; } = "Active";

    public string? CreateBy { get; set; }
}

public class DiscountUpdateDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Invalid product ID")]
    public int? ProductId { get; set; }

    [RegularExpression("^(Percentage|Fixed)$", ErrorMessage = "Discount type must be 'Percentage' or 'Fixed'")]
    public string? DiscountType { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Discount value must be non-negative")]
    public decimal? DiscountValue { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    [MaxLength(50)]
    public string? Status { get; set; }

    public string? UpdateBy { get; set; }
}

