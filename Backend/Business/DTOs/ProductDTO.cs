namespace Business.DTOs;

public class ProductResponseDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsActive { get; set; } // Sản phẩm có đang bán không
    public string? Status { get; set; } // Badge: New, Hot, Sell Well, Best Seller, ...
    public string? Description { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public string? CreateBy { get; set; }
    public string? UpdateBy { get; set; }
    public int? CategoryId { get; set; }
    public string? Code { get; set; }
    public string? Brand { get; set; } // Keep for backward compatibility
    public int? BrandId { get; set; }
    public string? BrandName { get; set; }
    public int Weight { get; set; } = 1000; // Trọng lượng sản phẩm (gram)
}

public class ProductCreateDto
{
    public string? Name { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Status { get; set; } // Badge: New, Hot, Sell Well, Best Seller, ...
    public string? Description { get; set; }
    public string? CreateBy { get; set; }
    public int? CategoryId { get; set; }
    public string? Code { get; set; }
    public string? Brand { get; set; } // Keep for backward compatibility
    public int? BrandId { get; set; }
    public int Weight { get; set; } = 1000; // Trọng lượng sản phẩm (gram)
}

public class ProductUpdateDto
{
    public string? Name { get; set; }
    public bool? IsActive { get; set; }
    public string? Status { get; set; } // Badge: New, Hot, Sell Well, Best Seller, ...
    public string? Description { get; set; }
    public string? UpdateBy { get; set; }
    public int? CategoryId { get; set; }
    public string? Code { get; set; }
    public string? Brand { get; set; } // Keep for backward compatibility
    public int? BrandId { get; set; }
    public int? Weight { get; set; } // Trọng lượng sản phẩm (gram)
}
