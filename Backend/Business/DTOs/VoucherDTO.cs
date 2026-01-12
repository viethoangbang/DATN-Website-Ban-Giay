namespace Business.DTOs;

// Voucher DTOs
public class VoucherResponseDto
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public decimal? Discount { get; set; }
    public decimal? MaxDiscount { get; set; }
    public int? Quantity { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? CategoryId { get; set; }
}

public class VoucherCreateDto
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public decimal? Discount { get; set; }
    public decimal? MaxDiscount { get; set; }
    public int? Quantity { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? CategoryId { get; set; }
    public string? CreateBy { get; set; }
}

public class VoucherUpdateDto
{
    public string? Name { get; set; }
    public string? Type { get; set; }
    public decimal? Discount { get; set; }
    public decimal? MaxDiscount { get; set; }
    public int? Quantity { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? UpdateBy { get; set; }
}
