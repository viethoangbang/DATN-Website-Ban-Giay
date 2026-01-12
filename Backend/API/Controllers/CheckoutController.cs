using Business.DTOs;
using Business.Services;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CheckoutController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ICheckoutService _checkoutService;
    private readonly IVNPayService _vnPayService;
    private readonly ILogger<CheckoutController> _logger;

    public CheckoutController(
        IOrderService orderService,
        ICheckoutService checkoutService,
        IVNPayService vnPayService,
        ILogger<CheckoutController> logger)
    {
        _orderService = orderService;
        _checkoutService = checkoutService;
        _vnPayService = vnPayService;
        _logger = logger;
    }

    /// <summary>
    /// Calculate shipping fee for delivery address
    /// </summary>
    [HttpPost("calculate-shipping-fee")]
    public async Task<IActionResult> CalculateShippingFee([FromBody] CalculateShippingFeeDto dto)
    {
        try
        {
            var customerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _checkoutService.CalculateShippingFeeAsync(customerId, dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating shipping fee");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Calculate voucher discount
    /// </summary>
    [HttpPost("calculate-voucher-discount")]
    public async Task<IActionResult> CalculateVoucherDiscount([FromBody] CalculateVoucherDiscountDto dto)
    {
        try
        {
            var result = await _checkoutService.CalculateVoucherDiscountAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating voucher discount");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Create order from cart
    /// </summary>
    [HttpPost("create-order")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
    {
        try
        {
            // Validate model state
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .SelectMany(x => x.Value!.Errors.Select(e => $"{x.Key}: {e.ErrorMessage}"))
                    .ToList();
                
                _logger.LogWarning($"Invalid model state: {string.Join(", ", errors)}");
                return BadRequest(new { message = "Invalid request data", errors });
            }

            var customerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            
            _logger.LogInformation($"Creating order for customer {customerId}, AddressId: {dto.AddressDeliveryId}, PaymentMethod: {dto.PaymentMethod}");
            
            var order = await _orderService.CreateAsync(customerId, dto);
            
            // If payment method is VNPay, return payment URL
            if (dto.PaymentMethod == "VNPay")
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
                var paymentUrl = await _checkoutService.CreatePaymentUrlAsync(order.Id, ipAddress);
                
                return Ok(new
                {
                    order,
                    paymentUrl = paymentUrl.PaymentUrl,
                    requiresPayment = true
                });
            }

            return Ok(new
            {
                order,
                paymentUrl = (string?)null,
                requiresPayment = false
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating order: {ex.Message}");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// VNPay payment callback (user return URL)
    /// </summary>
    [HttpGet("payment-callback")]
    [AllowAnonymous]
    public async Task<IActionResult> PaymentCallback()
    {
        try
        {
            var queryString = HttpContext.Request.QueryString.Value ?? "";
            _logger.LogInformation($"VNPay callback: {queryString}");

            // Parse VNPay response
            var responseData = _vnPayService.ParseQueryString(queryString);
            
            // Validate signature
            if (!_vnPayService.ValidateSignature(responseData))
            {
                _logger.LogWarning("VNPay callback signature validation failed");
                return Redirect("http://localhost:5175/checkout/error?message=Invalid signature");
            }

            var vnpResponseCode = responseData.GetValueOrDefault("vnp_ResponseCode", "");
            var vnpTxnRef = responseData.GetValueOrDefault("vnp_TxnRef", "");
            var vnpAmount = responseData.GetValueOrDefault("vnp_Amount", "");
            var vnpTransactionNo = responseData.GetValueOrDefault("vnp_TransactionNo", "");
            var vnpTransactionStatus = responseData.GetValueOrDefault("vnp_TransactionStatus", "");

            // Update order payment status
            if (vnpResponseCode == "00" && vnpTransactionStatus == "00")
            {
                // Payment successful
                _logger.LogInformation($"Payment successful for order {vnpTxnRef}, transaction: {vnpTransactionNo}");
                
                // Update order payment status
                try
                {
                    await _orderService.UpdatePaymentStatusAsync(vnpTxnRef, "Success", vnpTransactionNo);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error updating payment status for order {vnpTxnRef}");
                }
                
                // Redirect to success page
                return Redirect($"http://localhost:5175/checkout/success?orderCode={vnpTxnRef}");
            }
            else
            {
                // Payment failed
                _logger.LogWarning($"Payment failed for order {vnpTxnRef}, code: {vnpResponseCode}, status: {vnpTransactionStatus}");
                
                // Update order payment status
                try
                {
                    await _orderService.UpdatePaymentStatusAsync(vnpTxnRef, "Failed", vnpTransactionNo);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error updating payment status for order {vnpTxnRef}");
                }
                
                // Redirect to failure page
                return Redirect($"http://localhost:5175/checkout/failed?orderCode={vnpTxnRef}&code={vnpResponseCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing payment callback");
            return Redirect("http://localhost:5175/checkout/error");
        }
    }

    /// <summary>
    /// VNPay IPN (Instant Payment Notification) - Server to Server callback
    /// </summary>
    [HttpPost("payment-ipn")]
    [AllowAnonymous]
    public async Task<IActionResult> PaymentIpn()
    {
        try
        {
            // Read request body
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();
            
            _logger.LogInformation($"VNPay IPN received: {body}");

            // Parse VNPay response (can be query string or JSON)
            Dictionary<string, string> responseData;
            
            if (body.Contains("vnp_"))
            {
                // Query string format
                responseData = _vnPayService.ParseQueryString(body);
            }
            else
            {
                // Try to parse as JSON or query string from request
                var queryString = Request.QueryString.Value ?? "";
                responseData = _vnPayService.ParseQueryString(queryString);
            }

            // Validate signature
            if (!_vnPayService.ValidateSignature(responseData))
            {
                _logger.LogWarning("VNPay IPN signature validation failed");
                return BadRequest(new { RspCode = "97", Message = "Invalid signature" });
            }

            var vnpResponseCode = responseData.GetValueOrDefault("vnp_ResponseCode", "");
            var vnpTxnRef = responseData.GetValueOrDefault("vnp_TxnRef", "");
            var vnpTransactionNo = responseData.GetValueOrDefault("vnp_TransactionNo", "");
            var vnpTransactionStatus = responseData.GetValueOrDefault("vnp_TransactionStatus", "");
            var vnpAmount = responseData.GetValueOrDefault("vnp_Amount", "");

            // Update order payment status
            if (vnpResponseCode == "00" && vnpTransactionStatus == "00")
            {
                _logger.LogInformation($"IPN: Payment successful for order {vnpTxnRef}, transaction: {vnpTransactionNo}");
                
                try
                {
                    await _orderService.UpdatePaymentStatusAsync(vnpTxnRef, "Success", vnpTransactionNo);
                    return Ok(new { RspCode = "00", Message = "Success" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error updating payment status for order {vnpTxnRef}");
                    return Ok(new { RspCode = "99", Message = "Error updating order" });
                }
            }
            else
            {
                _logger.LogWarning($"IPN: Payment failed for order {vnpTxnRef}, code: {vnpResponseCode}");
                
                try
                {
                    await _orderService.UpdatePaymentStatusAsync(vnpTxnRef, "Failed", vnpTransactionNo);
                    return Ok(new { RspCode = "00", Message = "Updated" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error updating payment status for order {vnpTxnRef}");
                    return Ok(new { RspCode = "99", Message = "Error updating order" });
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing payment IPN");
            return BadRequest(new { RspCode = "99", Message = "Error processing IPN" });
        }
    }
}

