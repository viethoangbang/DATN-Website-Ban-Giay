using Business.DTOs;
using Business.Interfaces;
using Business.Exceptions;
using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Business.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepo _orderRepo;
    private readonly IOrderDetailRepo _orderDetailRepo;
    private readonly IProductDetailRepo _productDetailRepo;
    private readonly ICartRepo _cartRepo;
    private readonly ICartDetailRepo _cartDetailRepo;
    private readonly IAddressRepo _addressRepo;
    private readonly IDiscountRepo _discountRepo;
    private readonly IVoucherRepo _voucherRepo;
    private readonly ShopDbContext _context;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IOrderRepo orderRepo,
        IOrderDetailRepo orderDetailRepo,
        IProductDetailRepo productDetailRepo,
        ICartRepo cartRepo,
        ICartDetailRepo cartDetailRepo,
        IAddressRepo addressRepo,
        IDiscountRepo discountRepo,
        IVoucherRepo voucherRepo,
        ShopDbContext context,
        ILogger<OrderService> logger)
    {
        _orderRepo = orderRepo;
        _orderDetailRepo = orderDetailRepo;
        _productDetailRepo = productDetailRepo;
        _cartRepo = cartRepo;
        _cartDetailRepo = cartDetailRepo;
        _addressRepo = addressRepo;
        _discountRepo = discountRepo;
        _voucherRepo = voucherRepo;
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<OrderResponseDto>> GetAllAsync()
    {
        var orders = await _orderRepo.GetAllAsync();
        return orders.Select(MapToDto);
    }

    public async Task<OrderResponseDto?> GetByIdAsync(int id)
    {
        var order = await _orderRepo.GetByIdAsync(id);
        if (order == null)
            throw new NotFoundException("Order", id);
        return MapToDto(order);
    }

    public async Task<IEnumerable<OrderResponseDto>> GetMyOrdersAsync(int customerId)
    {
        var orders = await _orderRepo.GetByCustomerIdAsync(customerId);
        return orders.Select(MapToDto);
    }

    // Main checkout method - creates order from cart
    public async Task<OrderResponseDto> CreateAsync(int customerId, CreateOrderDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _logger.LogInformation($"Creating order for customer {customerId}, AddressId: {dto.AddressDeliveryId}, PaymentMethod: {dto.PaymentMethod}");

            // 1. Get customer's cart
            var cart = await _cartRepo.GetActiveCartByAccountIdAsync(customerId);
            if (cart == null || cart.CartDetails == null || !cart.CartDetails.Any())
            {
                _logger.LogWarning($"Cart is empty for customer {customerId}");
                throw new BadRequestException("Cart is empty");
            }

            _logger.LogInformation($"Cart has {cart.CartDetails.Count} items");

            // 2. Validate address
            var address = await _addressRepo.GetByIdAsync(dto.AddressDeliveryId);
            if (address == null)
            {
                _logger.LogWarning($"Address {dto.AddressDeliveryId} not found");
                throw new BadRequestException("Invalid delivery address");
            }
            
            if (address.AccountId != customerId)
            {
                _logger.LogWarning($"Address {dto.AddressDeliveryId} does not belong to customer {customerId}");
                throw new BadRequestException("Invalid delivery address");
            }

            // 3. Validate and calculate items with discount
            decimal productTotal = 0;
            decimal productDiscount = 0;
            int totalWeight = 0;
            var orderDetails = new List<OrderDetail>();

            foreach (var cartItem in cart.CartDetails)
            {
                var productDetail = await _productDetailRepo.GetByIdAsync(cartItem.ProductDetailId ?? 0);
                if (productDetail == null)
                {
                    _logger.LogError($"ProductDetail {cartItem.ProductDetailId} not found");
                    throw new NotFoundException("ProductDetail", cartItem.ProductDetailId ?? 0);
                }

                // Check stock
                var requestedQuantity = cartItem.Quantity ?? 0;
                var availableStock = productDetail.Quantity ?? 0;
                if (availableStock < requestedQuantity)
                {
                    _logger.LogWarning($"Insufficient stock for ProductDetail {productDetail.Id}: requested {requestedQuantity}, available {availableStock}");
                    throw new InsufficientStockException(
                        productDetail.Id,
                        requestedQuantity,
                        availableStock
                    );
                }

                // Get original price
                decimal originalPrice = productDetail.Price ?? 0;
                decimal finalPrice = originalPrice;

                // Calculate discount for this product
                var discountInfo = await CalculateProductDiscount(productDetail.ProductId ?? 0, originalPrice);
                if (discountInfo != null)
                {
                    finalPrice = discountInfo.FinalPrice;
                    productDiscount += (originalPrice - finalPrice) * requestedQuantity;
                }

                productTotal += originalPrice * requestedQuantity;

                // Calculate weight (get from Product table)
                var product = productDetail.Product;
                if (product != null)
                {
                    totalWeight += (product.Weight * requestedQuantity);
                }
                else
                {
                    // Default weight if product not loaded
                    totalWeight += (1000 * requestedQuantity); // 1kg default
                    _logger.LogWarning($"Product not loaded for ProductDetail {productDetail.Id}, using default weight 1000g");
                }

                // Create order detail
                orderDetails.Add(new OrderDetail
                {
                    ProductDetailId = productDetail.Id,
                    Quantity = cartItem.Quantity,
                    ProductName = productDetail.Product?.Name ?? "Unknown",
                    OriginalPrice = originalPrice,
                    FinalPrice = finalPrice,
                    Price = finalPrice, // For backward compatibility
                    DiscountType = discountInfo?.DiscountType,
                    DiscountValue = discountInfo?.DiscountValue,
                    CreateDate = DateTime.Now
                });
            }

            // 4. Apply voucher if provided
            decimal voucherDiscount = 0;
            if (!string.IsNullOrWhiteSpace(dto.VoucherCode))
            {
                var voucherCode = dto.VoucherCode.Trim();
                _logger.LogInformation($"Applying voucher code: {voucherCode}");
                
                var voucher = await _voucherRepo.GetByCodeAsync(voucherCode);
                if (voucher == null)
                {
                    _logger.LogWarning($"Voucher code not found: {voucherCode}");
                    throw new BadRequestException($"Mã giảm giá '{voucherCode}' không tồn tại");
                }
                
                if (voucher.Status != "Active")
                {
                    _logger.LogWarning($"Voucher {voucherCode} is not active. Status: {voucher.Status}");
                    throw new BadRequestException($"Mã giảm giá '{voucherCode}' không khả dụng");
                }
                
                var now = DateTime.Now;
                if (voucher.StartDate > now)
                {
                    _logger.LogWarning($"Voucher {voucherCode} not yet valid. StartDate: {voucher.StartDate}, Now: {now}");
                    throw new BadRequestException($"Mã giảm giá '{voucherCode}' chưa có hiệu lực");
                }
                
                if (voucher.EndDate < now)
                {
                    _logger.LogWarning($"Voucher {voucherCode} has expired. EndDate: {voucher.EndDate}, Now: {now}");
                    throw new BadRequestException($"Mã giảm giá '{voucherCode}' đã hết hạn");
                }
                
                // Check voucher quantity
                if (voucher.Quantity.HasValue && voucher.Quantity.Value <= 0)
                {
                    _logger.LogWarning($"Voucher {voucherCode} has no remaining quantity");
                    throw new BadRequestException($"Mã giảm giá '{voucherCode}' đã hết lượt sử dụng");
                }
                
                // Calculate voucher discount
                var subtotalAfterProductDiscount = productTotal - productDiscount;
                if (voucher.Type == "Percentage")
                {
                    voucherDiscount = subtotalAfterProductDiscount * (voucher.Discount ?? 0) / 100;
                    if (voucher.MaxDiscount.HasValue)
                    {
                        voucherDiscount = Math.Min(voucherDiscount, voucher.MaxDiscount.Value);
                    }
                }
                else if (voucher.Type == "Fixed")
                {
                    voucherDiscount = Math.Min(voucher.Discount ?? 0, subtotalAfterProductDiscount);
                }
                else
                {
                    _logger.LogWarning($"Unknown voucher type: {voucher.Type}");
                    throw new BadRequestException($"Loại mã giảm giá không hợp lệ");
                }
                
                _logger.LogInformation($"Voucher {voucherCode} applied successfully. Discount: {voucherDiscount} VND");
            }

            // 5. Get shipping fee from DTO (calculated by frontend via GHN API)
            decimal shippingFee = dto.ShippingFee;
            if (shippingFee < 0)
            {
                _logger.LogWarning($"Invalid shipping fee {shippingFee} from DTO, using default 0");
                shippingFee = 0;
            }

            // 6. Calculate final total
            decimal total = productTotal - productDiscount - voucherDiscount + shippingFee;
            if (total < 0) total = 0;

            // 7. Generate order code
            string orderCode = $"ORD{DateTime.Now:yyyyMMddHHmmss}{customerId}";

            // 8. Create order
            var order = new Order
            {
                CustomerId = customerId,
                Code = orderCode,
                Description = dto.Note,
                AddressDeliveryId = dto.AddressDeliveryId,
                ReceiverName = address.ReceiverName,
                ReceiverPhone = address.ReceiverPhone,
                ReceiverAddress = $"{address.WardName}, {address.DistrictName}, {address.ProvinceName}",
                ProductTotal = productTotal,
                ProductDiscount = productDiscount,
                VoucherDiscount = voucherDiscount,
                VoucherCode = dto.VoucherCode,
                ShippingFee = shippingFee,
                Total = total,
                ActualWeight = totalWeight,
                PaymentMethod = dto.PaymentMethod,
                PaymentStatus = "Pending",
                ShippingStatus = "Pending",
                Status = "Pending",
                Note = dto.Note,
                CreateDate = DateTime.Now
            };

            var createdOrder = await _orderRepo.AddAsync(order);

            // 9. Add order details and reduce stock
            foreach (var orderDetail in orderDetails)
            {
                orderDetail.OrderId = createdOrder.Id;
                await _orderDetailRepo.AddAsync(orderDetail);

                // Reduce stock
                var productDetail = await _productDetailRepo.GetByIdAsync(orderDetail.ProductDetailId ?? 0);
                if (productDetail != null)
                {
                    productDetail.Quantity -= orderDetail.Quantity;
                    await _productDetailRepo.UpdateAsync(productDetail);
                }
            }

            // 10. Clear cart
            cart.EndDate = DateTime.Now;
            await _cartRepo.UpdateAsync(cart);

            await transaction.CommitAsync();

            _logger.LogInformation($"Order {orderCode} created successfully for customer {customerId}");

            var result = await _orderRepo.GetByIdAsync(createdOrder.Id);
            return MapToDto(result!);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error creating order for customer {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<bool> UpdateStatusAsync(int id, UpdateOrderStatusDto dto)
    {
        var order = await _orderRepo.GetByIdAsync(id);
        if (order == null)
            throw new NotFoundException("Order", id);

        // Validate: Cannot change status if order is already cancelled
        if (order.Status?.Equals("Cancelled", StringComparison.OrdinalIgnoreCase) == true)
        {
            throw new BadRequestException("Không thể thay đổi trạng thái đơn hàng đã bị hủy");
        }

        var oldStatus = order.Status;
        
        // Validation: Strict status flow
        if (!IsValidStatusTransition(oldStatus, dto.Status))
        {
             throw new BadRequestException($"Không thể chuyển trạng thái từ {oldStatus} sang {dto.Status}");
        }

        order.Status = dto.Status;
        order.UpdateDate = DateTime.Now;

        await _orderRepo.UpdateAsync(order);

        // Add history record
        var history = new OrderStatusHistory
        {
            OrderId = order.Id,
            FromStatus = oldStatus,
            ToStatus = dto.Status,
            Note = dto.Note,
            CreatedBy = "Employee", // TODO: Get from claims if possible, or pass in
            CreateDate = DateTime.Now
        };
        
        _context.OrderStatusHistories.Add(history);
        await _context.SaveChangesAsync();

        // Log status change (could save to OrderStatusHistory table)
        _logger.LogInformation(
            $"Order {order.Code} status changed from {oldStatus} to {dto.Status}"
        );

        return true;
    }

    public async Task<bool> CancelOrderAsync(int orderId, int customerId)
    {
        var order = await _orderRepo.GetByIdAsync(orderId);
        if (order == null)
            throw new NotFoundException("Order", orderId);

        // Validate: Order must belong to the customer
        if (order.CustomerId != customerId)
        {
            throw new BadRequestException("Không có quyền hủy đơn hàng này");
        }

        // Validate: Cannot cancel if already cancelled
        if (order.Status?.Equals("Cancelled", StringComparison.OrdinalIgnoreCase) == true)
        {
            throw new BadRequestException("Đơn hàng đã bị hủy");
        }

        // Validate: Only allow cancellation for Pending or Confirmed orders
        if (order.Status != "Pending" && order.Status != "Confirmed")
        {
            throw new BadRequestException($"Không thể hủy đơn hàng ở trạng thái {order.Status}");
        }

        var oldStatus = order.Status;
        order.Status = "Cancelled";
        order.UpdateDate = DateTime.Now;

        await _orderRepo.UpdateAsync(order);

        _logger.LogInformation(
            $"Order {order.Code} cancelled by customer {customerId}, status changed from {oldStatus} to Cancelled"
        );

        return true;
    }

    public async Task<bool> UpdatePaymentStatusAsync(string orderCode, string paymentStatus, string? transactionId = null)
    {
        try
        {
            // Find order by code
            var order = await _context.Orders
                .Include(o => o.Payments)
                .FirstOrDefaultAsync(o => o.Code == orderCode);

            if (order == null)
            {
                _logger.LogWarning($"Order not found with code: {orderCode}");
                return false;
            }

            var oldPaymentStatus = order.PaymentStatus;
            order.PaymentStatus = paymentStatus;
            order.UpdateDate = DateTime.Now;

            // Update or create Payment record
            var payment = order.Payments?.FirstOrDefault();
            if (payment == null)
            {
                // Create new Payment record
                payment = new Payment
                {
                    OrderId = order.Id,
                    PaymentMethod = order.PaymentMethod ?? "VNPay",
                    Amount = order.Total ?? 0,
                    CreateDate = DateTime.Now,
                    TransactionNo = transactionId,
                    Status = paymentStatus
                };
                await _context.Payments.AddAsync(payment);
            }
            else
            {
                // Update existing Payment record
                payment.TransactionNo = transactionId ?? payment.TransactionNo;
                payment.UpdateDate = DateTime.Now;
                payment.Status = paymentStatus;
                if (paymentStatus == "Success")
                {
                    payment.PaidDate = DateTime.Now;
                }
                _context.Payments.Update(payment);
            }

            // If payment successful, update order status
            if (paymentStatus == "Success" && order.Status == "Pending")
            {
                order.Status = "Confirmed";
            }

            await _orderRepo.UpdateAsync(order);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                $"Order {orderCode} payment status changed from {oldPaymentStatus} to {paymentStatus}"
            );

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating payment status for order {orderCode}");
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var order = await _orderRepo.GetByIdAsync(id);
        if (order == null)
            throw new NotFoundException("Order", id);

        await _orderRepo.DeleteAsync(order);
        return true;
    }

    private OrderResponseDto MapToDto(Order order)
    {
        return new OrderResponseDto
        {
            Id = order.Id,
            Code = order.Code,
            Description = order.Description,
            ProductTotal = order.ProductTotal,
            ProductDiscount = order.ProductDiscount,
            VoucherDiscount = order.VoucherDiscount,
            ShippingFee = order.ShippingFee,
            Total = order.Total,
            VoucherCode = order.VoucherCode,
            Status = order.Status,
            PaymentStatus = order.PaymentStatus,
            ShippingStatus = order.ShippingStatus,
            PaymentMethod = order.PaymentMethod,
            CreateDate = order.CreateDate,
            UpdateDate = order.UpdateDate,
            DeliveryDate = order.DeliveryDate,
            EstimatedDeliveryDate = order.EstimatedDeliveryDate,
            CustomerId = order.CustomerId,
            CustomerName = order.Customer?.FullName,
            CustomerEmail = order.Customer?.Email,
            EmployeeId = order.EmployeeId,
            EmployeeName = order.Employee?.FullName,
            AddressDeliveryId = order.AddressDeliveryId,
            ReceiverName = order.ReceiverName,
            ReceiverPhone = order.ReceiverPhone,
            ReceiverAddress = order.ReceiverAddress,
            ActualWeight = order.ActualWeight,
            Note = order.Note,
            Items = order.OrderDetails?.Select(od => new OrderDetailResponseDto
            {
                Id = od.Id,
                ProductDetailId = od.ProductDetailId,
                ProductName = od.ProductName,
                Quantity = od.Quantity,
                OriginalPrice = od.OriginalPrice,
                FinalPrice = od.FinalPrice,
                Price = od.Price,
                DiscountType = od.DiscountType,
                DiscountValue = od.DiscountValue,
                ImageUrl = od.ProductDetail?.Image?.Url ?? od.ProductDetail?.ProductDetailImages?.OrderBy(pdi => pdi.DisplayOrder ?? 0).FirstOrDefault()?.Image?.Url,
                CreateDate = od.CreateDate
            }).ToList() ?? new List<OrderDetailResponseDto>(),
            StatusHistories = order.StatusHistories?.Select(h => new OrderStatusHistoryDto
            {
                Id = h.Id,
                FromStatus = h.FromStatus,
                ToStatus = h.ToStatus,
                Note = h.Note,
                CreatedBy = h.CreatedBy,
                CreateDate = h.CreateDate
            }).OrderByDescending(h => h.CreateDate).ToList() ?? new List<OrderStatusHistoryDto>()
        };
    }

    private async Task<DiscountCalculationResult?> CalculateProductDiscount(int productId, decimal originalPrice)
    {
        var now = DateTime.Now;

        var activeDiscounts = await _discountRepo.GetActiveByProductIdAsync(productId);
        activeDiscounts = activeDiscounts
            .Where(d => d.Status == "Active" && d.StartDate <= now && d.EndDate >= now)
            .ToList();

        if (!activeDiscounts.Any())
            return null;

        // Find best discount
        Discount? bestDiscount = null;
        decimal maxDiscountAmount = 0;

        foreach (var discount in activeDiscounts)
        {
            decimal discountAmount = 0;

            if (discount.DiscountType == "Percentage")
            {
                discountAmount = originalPrice * (discount.DiscountValue / 100);
            }
            else if (discount.DiscountType == "Fixed")
            {
                discountAmount = discount.DiscountValue;
            }

            if (discountAmount > maxDiscountAmount)
            {
                maxDiscountAmount = discountAmount;
                bestDiscount = discount;
            }
        }

        if (bestDiscount == null)
            return null;

        decimal finalPrice = originalPrice - maxDiscountAmount;
        if (finalPrice < 0)
            finalPrice = 0;

        return new DiscountCalculationResult
        {
            DiscountType = bestDiscount.DiscountType,
            DiscountValue = bestDiscount.DiscountValue,
            FinalPrice = finalPrice
        };
    }

    private class DiscountCalculationResult
    {
        public string DiscountType { get; set; } = null!;
        public decimal DiscountValue { get; set; }
        public decimal FinalPrice { get; set; }
    }
    private bool IsValidStatusTransition(string? currentStatus, string newStatus)
    {
        if (currentStatus == newStatus) return true;
        
        // Strict flow: Pending -> Processing -> Shipping -> Delivered
        // Cancelled is end state
        
        return (currentStatus, newStatus) switch
        {
            ("Pending", "Processing") => true,
            ("Pending", "Cancelled") => true,
            ("Processing", "Shipping") => true,
            ("Processing", "Cancelled") => true, // Allowed to cancel during processing?
            ("Shipping", "Delivered") => true,
            ("Shipping", "Cancelled") => true, // Return to seller?
            ("Delivered", _) => false, // End state
            ("Cancelled", _) => false, // End state
            _ => false
        };
    }
}

