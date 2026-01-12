namespace Business.DTOs;

// ProductDetail DTOs
public class ProductDetailResponseDto
{
    public int Id { get; set; }
    public decimal? Price { get; set; } // Giá gốc
    public int? Quantity { get; set; }
    public int? ImageId { get; set; }
    public int? ColorId { get; set; }
    public int? SizeId { get; set; }
    public int? ProductId { get; set; }
    public DateTime? CreateDate { get; set; }
    public string? ColorName { get; set; }
    public string? SizeName { get; set; }
    public string? ImageUrl { get; set; }
    public List<ImageResponseDto> Images { get; set; } = new();
    
    // Discount fields (tính từ backend)
    public string? DiscountType { get; set; } // "Percentage" or "Fixed"
    public decimal? DiscountValue { get; set; }
    public decimal? FinalPrice { get; set; } // Giá sau discount
    public bool DiscountActive { get; set; } // Có discount active không
}

public class ProductDetailCreateDto
{
    public decimal? Price { get; set; }
    public int? Quantity { get; set; }
    public int? ImageId { get; set; }
    public int? ColorId { get; set; }
    public int? SizeId { get; set; }
    public int? ProductId { get; set; }
    public List<int>? ImageIds { get; set; }
}

public class ProductDetailUpdateDto
{
    public decimal? Price { get; set; }
    public int? Quantity { get; set; }
    public int? ImageId { get; set; }
    public int? ColorId { get; set; }
    public int? SizeId { get; set; }
    public List<int>? ImageIds { get; set; }
}
