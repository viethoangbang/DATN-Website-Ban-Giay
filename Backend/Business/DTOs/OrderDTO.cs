using System.ComponentModel.DataAnnotations;

namespace Business.DTOs;

// Order Response DTO
public class OrderResponseDto
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public string? Code { get; set; }
    
    // Pricing
    public decimal? ProductTotal { get; set; } // Tổng tiền sản phẩm (chưa discount)
    public decimal? ProductDiscount { get; set; } // Discount từ product
    public decimal? VoucherDiscount { get; set; } // Discount từ voucher
    public decimal? ShippingFee { get; set; }
    public decimal? Total { get; set; } // Tổng cuối cùng
    public string? VoucherCode { get; set; }
    
    // Status
    public string? Status { get; set; }
    public string? PaymentStatus { get; set; }
    public string? ShippingStatus { get; set; }
    public string? PaymentMethod { get; set; }
    
    // Dates
    public DateTime? CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime? EstimatedDeliveryDate { get; set; }
    
    // Customer & Employee
    public int? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public int? EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    
    // Delivery info
    public int? AddressDeliveryId { get; set; }
    public string? ReceiverName { get; set; }
    public string? ReceiverPhone { get; set; }
    public string? ReceiverAddress { get; set; }
    
    public int? ActualWeight { get; set; }
    public string? Note { get; set; }
    
    // Status History
    public List<OrderStatusHistoryDto> StatusHistories { get; set; } = new();

    // Order items
    public List<OrderDetailResponseDto> Items { get; set; } = new();
}

// Order Detail Response DTO
public class OrderDetailResponseDto
{
    public int Id { get; set; }
    public int? ProductDetailId { get; set; }
    public string? ProductName { get; set; }
    public int? Quantity { get; set; }
    
    // Pricing
    public decimal? OriginalPrice { get; set; } // Giá gốc
    public decimal? FinalPrice { get; set; } // Giá sau discount
    public decimal? Price { get; set; } // Backward compatible (same as FinalPrice)
    
    // Discount info
    public string? DiscountType { get; set; }
    public decimal? DiscountValue { get; set; }
    
    // Image
    public string? ImageUrl { get; set; }
    
    // Variant info
    public string? Size { get; set; }
    public string? Color { get; set; }
    
    public DateTime? CreateDate { get; set; }
}

// Create Order DTO
public class CreateOrderDto
{
    [Required(ErrorMessage = "Delivery address is required")]
    public int AddressDeliveryId { get; set; }
    
    public string? VoucherCode { get; set; }
    
    [Required(ErrorMessage = "Payment method is required")]
    public string PaymentMethod { get; set; } = "COD"; // VNPay, COD
    
    [Required(ErrorMessage = "Shipping fee is required")]
    public decimal ShippingFee { get; set; }
    
    public string? Note { get; set; }
    
    // Items will be taken from cart
}

// Update Order Status DTO
public class UpdateOrderStatusDto
{
    [Required(ErrorMessage = "Status is required")]
    public string Status { get; set; } = null!;
    
    public string? Note { get; set; }
}

// Calculate Shipping Fee Request
public class CalculateShippingFeeDto
{
    [Required]
    public int AddressDeliveryId { get; set; }
}

// Calculate Shipping Fee Response
public class ShippingFeeResponseDto
{
    public decimal Fee { get; set; }
    public string? ServiceName { get; set; }
    public int? EstimatedDays { get; set; }
    public string? Message { get; set; }
}

// VNPay Payment URL Response
public class PaymentUrlResponseDto
{
    public string PaymentUrl { get; set; } = null!;
    public string OrderCode { get; set; } = null!;
}

// Calculate Voucher Discount Request
public class CalculateVoucherDiscountDto
{
    [Required]
    public string VoucherCode { get; set; } = null!;
    
    [Required]
    public decimal Subtotal { get; set; }
}

// Calculate Voucher Discount Response
public class VoucherDiscountResponseDto
{
    public decimal Discount { get; set; }
    public string? VoucherName { get; set; }
    public string? Message { get; set; }
    public bool IsValid { get; set; }
}

// Order Status History DTO
public class OrderStatusHistoryDto
{
    public int Id { get; set; }
    public string? FromStatus { get; set; }
    public string? ToStatus { get; set; }
    public string? Note { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreateDate { get; set; }
}
