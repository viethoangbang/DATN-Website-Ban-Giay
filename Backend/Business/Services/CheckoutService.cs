using Business.DTOs;
using Business.Exceptions;
using Data.Interfaces;
using Microsoft.Extensions.Logging;

namespace Business.Services;

public interface ICheckoutService
{
    Task<ShippingFeeResponseDto> CalculateShippingFeeAsync(int customerId, CalculateShippingFeeDto dto);
    Task<PaymentUrlResponseDto> CreatePaymentUrlAsync(int orderId, string ipAddress);
    Task<VoucherDiscountResponseDto> CalculateVoucherDiscountAsync(CalculateVoucherDiscountDto dto);
}

public class CheckoutService : ICheckoutService
{
    private readonly IAddressRepo _addressRepo;
    private readonly IOrderRepo _orderRepo;
    private readonly IGHNService _ghnService;
    private readonly IVNPayService _vnPayService;
    private readonly IVoucherRepo _voucherRepo;
    private readonly ILogger<CheckoutService> _logger;

    public CheckoutService(
        IAddressRepo addressRepo,
        IOrderRepo orderRepo,
        IGHNService ghnService,
        IVNPayService vnPayService,
        IVoucherRepo voucherRepo,
        ILogger<CheckoutService> logger)
    {
        _addressRepo = addressRepo;
        _orderRepo = orderRepo;
        _ghnService = ghnService;
        _vnPayService = vnPayService;
        _voucherRepo = voucherRepo;
        _logger = logger;
    }

    public async Task<ShippingFeeResponseDto> CalculateShippingFeeAsync(int customerId, CalculateShippingFeeDto dto)
    {
        try
        {
            // Get delivery address
            var address = await _addressRepo.GetByIdAsync(dto.AddressDeliveryId);
            if (address == null || address.AccountId != customerId)
            {
                throw new BadRequestException("Invalid delivery address");
            }

            // For now, use default weight and value
            // In production, calculate from actual cart items
            int estimatedWeight = 1000; // 1kg default
            int insuranceValue = 500000; // 500k VND default

            // Use GHN district ID and ward code from address
            var districtId = address.DistrictId?.ToString() ?? "0";
            var wardCode = address.WardCode ?? "";

            _logger.LogInformation($"Calculating shipping fee for address {dto.AddressDeliveryId}: DistrictId={districtId}, WardCode={wardCode}");

            if (address.DistrictId == null || address.DistrictId == 0)
            {
                _logger.LogWarning($"Address {dto.AddressDeliveryId} missing DistrictId. Address may need to be updated with GHN location IDs.");
            }

            if (string.IsNullOrEmpty(address.WardCode))
            {
                _logger.LogWarning($"Address {dto.AddressDeliveryId} missing WardCode. Address may need to be updated with GHN location IDs.");
            }

            var shippingFee = await _ghnService.CalculateShippingFeeAsync(
                districtId,
                wardCode,
                estimatedWeight,
                insuranceValue
            );

            _logger.LogInformation($"Shipping fee calculated: {shippingFee.Fee} VND");

            return shippingFee;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error calculating shipping fee for customer {customerId}");
            throw;
        }
    }

    public async Task<PaymentUrlResponseDto> CreatePaymentUrlAsync(int orderId, string ipAddress)
    {
        try
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new NotFoundException("Order", orderId);
            }

            if (order.PaymentMethod != "VNPay")
            {
                throw new BadRequestException("Order payment method is not VNPay");
            }

            var orderInfo = $"Thanh toan don hang {order.Code}";
            var paymentUrl = _vnPayService.CreatePaymentUrl(
                order.Code ?? orderId.ToString(),
                order.Total ?? 0,
                orderInfo,
                ipAddress
            );

            return new PaymentUrlResponseDto
            {
                PaymentUrl = paymentUrl,
                OrderCode = order.Code ?? orderId.ToString()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating payment URL for order {orderId}");
            throw;
        }
    }

    public async Task<VoucherDiscountResponseDto> CalculateVoucherDiscountAsync(CalculateVoucherDiscountDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.VoucherCode))
            {
                return new VoucherDiscountResponseDto
                {
                    Discount = 0,
                    IsValid = false,
                    Message = "Mã giảm giá không được để trống"
                };
            }

            var voucherCode = dto.VoucherCode.Trim();
            _logger.LogInformation($"Calculating voucher discount for code: {voucherCode}, Subtotal: {dto.Subtotal}");

            var voucher = await _voucherRepo.GetByCodeAsync(voucherCode);
            if (voucher == null)
            {
                _logger.LogWarning($"Voucher code not found: {voucherCode}");
                return new VoucherDiscountResponseDto
                {
                    Discount = 0,
                    IsValid = false,
                    Message = $"Mã giảm giá '{voucherCode}' không tồn tại"
                };
            }

            if (voucher.Status != "Active")
            {
                _logger.LogWarning($"Voucher {voucherCode} is not active. Status: {voucher.Status}");
                return new VoucherDiscountResponseDto
                {
                    Discount = 0,
                    IsValid = false,
                    Message = $"Mã giảm giá '{voucherCode}' không khả dụng"
                };
            }

            var now = DateTime.Now;
            if (voucher.StartDate > now)
            {
                _logger.LogWarning($"Voucher {voucherCode} not yet valid. StartDate: {voucher.StartDate}, Now: {now}");
                return new VoucherDiscountResponseDto
                {
                    Discount = 0,
                    IsValid = false,
                    Message = $"Mã giảm giá '{voucherCode}' chưa có hiệu lực"
                };
            }

            if (voucher.EndDate < now)
            {
                _logger.LogWarning($"Voucher {voucherCode} has expired. EndDate: {voucher.EndDate}, Now: {now}");
                return new VoucherDiscountResponseDto
                {
                    Discount = 0,
                    IsValid = false,
                    Message = $"Mã giảm giá '{voucherCode}' đã hết hạn"
                };
            }

            if (voucher.Quantity.HasValue && voucher.Quantity.Value <= 0)
            {
                _logger.LogWarning($"Voucher {voucherCode} has no remaining quantity");
                return new VoucherDiscountResponseDto
                {
                    Discount = 0,
                    IsValid = false,
                    Message = $"Mã giảm giá '{voucherCode}' đã hết lượt sử dụng"
                };
            }

            // Calculate discount
            decimal discount = 0;
            if (voucher.Type == "Percentage")
            {
                discount = dto.Subtotal * (voucher.Discount ?? 0) / 100;
                if (voucher.MaxDiscount.HasValue)
                {
                    discount = Math.Min(discount, voucher.MaxDiscount.Value);
                }
            }
            else if (voucher.Type == "Fixed")
            {
                discount = Math.Min(voucher.Discount ?? 0, dto.Subtotal);
            }
            else
            {
                _logger.LogWarning($"Unknown voucher type: {voucher.Type}");
                return new VoucherDiscountResponseDto
                {
                    Discount = 0,
                    IsValid = false,
                    Message = "Loại mã giảm giá không hợp lệ"
                };
            }

            _logger.LogInformation($"Voucher {voucherCode} applied successfully. Discount: {discount} VND");

            return new VoucherDiscountResponseDto
            {
                Discount = discount,
                VoucherName = voucher.Name,
                IsValid = true,
                Message = $"Áp dụng mã giảm giá '{voucherCode}' thành công"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error calculating voucher discount for code: {dto.VoucherCode}");
            return new VoucherDiscountResponseDto
            {
                Discount = 0,
                IsValid = false,
                Message = "Có lỗi xảy ra khi tính toán mã giảm giá"
            };
        }
    }
}

