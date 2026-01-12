using System.ComponentModel.DataAnnotations;

namespace Business.DTOs;

// Cart DTOs with validation
public class CartResponseDto
{
    public int Id { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? AccountId { get; set; }
    public List<CartDetailResponseDto> Items { get; set; } = new();
}

public class CartDetailResponseDto
{
    public int Id { get; set; }
    public int? Quantity { get; set; }
    public int? ProductDetailId { get; set; }
    public string? ProductName { get; set; }
    public decimal? Price { get; set; } // Giá gốc (từ ProductDetail.Price)
    public decimal? FinalPrice { get; set; } // Giá sau discount (đã tính từ backend)
    public string? ColorName { get; set; }
    public string? SizeName { get; set; }
    public string? ImageUrl { get; set; }
    public string? DiscountType { get; set; } // "Percentage" or "Fixed"
    public decimal? DiscountValue { get; set; }
    public bool DiscountActive { get; set; } // Có discount active không
}

public class AddToCartDto
{
    [Required(ErrorMessage = "Product detail ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid product detail ID")]
    public int ProductDetailId { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
    public int Quantity { get; set; }
}

public class UpdateCartItemDto
{
    [Required(ErrorMessage = "Quantity is required")]
    [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
    public int Quantity { get; set; }
}
