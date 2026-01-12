using Business.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Business.Services;

public interface IVNPayService
{
    string CreatePaymentUrl(string orderCode, decimal amount, string orderInfo, string ipAddress);
    bool ValidateSignature(Dictionary<string, string> responseData);
    Dictionary<string, string> ParseQueryString(string queryString);
}

public class VNPayService : IVNPayService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<VNPayService> _logger;

    public VNPayService(IConfiguration configuration, ILogger<VNPayService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public string CreatePaymentUrl(string orderCode, decimal amount, string orderInfo, string ipAddress)
    {
        try
        {
            var tmnCode = _configuration["VNPay:TmnCode"];
            var hashSecret = _configuration["VNPay:HashSecret"];
            var baseUrl = _configuration["VNPay:BaseUrl"];
            var returnUrl = _configuration["VNPay:ReturnUrl"];
            var version = _configuration["VNPay:Version"] ?? "2.1.0";
            var command = _configuration["VNPay:Command"] ?? "pay";
            var currCode = _configuration["VNPay:CurrCode"] ?? "VND";
            var locale = _configuration["VNPay:Locale"] ?? "vn";

            // Normalize IP address (convert IPv6 localhost to IPv4)
            var normalizedIp = NormalizeIpAddress(ipAddress);

            // VNPay requires amount in VND * 100 (smallest unit)
            var vnpAmount = ((long)(amount * 100)).ToString();

            var vnpParams = new SortedDictionary<string, string>
            {
                { "vnp_Version", version },
                { "vnp_Command", command },
                { "vnp_TmnCode", tmnCode ?? "" },
                { "vnp_Amount", vnpAmount },
                { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
                { "vnp_CurrCode", currCode },
                { "vnp_IpAddr", normalizedIp },
                { "vnp_Locale", locale },
                { "vnp_OrderInfo", orderInfo },
                { "vnp_OrderType", "other" },
                { "vnp_ReturnUrl", returnUrl ?? "" },
                { "vnp_TxnRef", orderCode }
            };

            // Note: VNPay IPN URL is configured in merchant dashboard, not in request parameters

            // Build query string for signature (URL encoded, then replace %20 with +)
            // VNPay requires: encode URI component, then replace %20 with + for hash calculation
            var signDataBuilder = new StringBuilder();
            foreach (var kvp in vnpParams)
            {
                if (!string.IsNullOrEmpty(kvp.Value))
                {
                    // URL encode first
                    var encodedValue = HttpUtility.UrlEncode(kvp.Value);
                    // Replace %20 with + (VNPay requirement)
                    encodedValue = encodedValue.Replace("%20", "+");
                    signDataBuilder.Append($"{kvp.Key}={encodedValue}&");
                }
            }

            var signData = signDataBuilder.ToString().TrimEnd('&');

            // Generate secure hash using SHA256 on the encoded query string
            var vnpSecureHash = HmacSHA256(hashSecret ?? "", signData);

            // Build query string for URL (WITH URL encoding, keep %20 as is)
            var queryBuilder = new StringBuilder();
            foreach (var kvp in vnpParams)
            {
                if (!string.IsNullOrEmpty(kvp.Value))
                {
                    queryBuilder.Append($"{kvp.Key}={HttpUtility.UrlEncode(kvp.Value)}&");
                }
            }

            var queryString = queryBuilder.ToString().TrimEnd('&');
            
            // Add SecureHash to URL
            var paymentUrl = $"{baseUrl}?{queryString}&vnp_SecureHash={vnpSecureHash}";

            _logger.LogInformation($"Created VNPay payment URL for order {orderCode}");
            _logger.LogInformation($"Sign data (for hash): {signData}");
            _logger.LogInformation($"Query string (for URL): {queryString}");
            _logger.LogInformation($"Hash secret length: {(hashSecret ?? "").Length}");
            _logger.LogInformation($"Generated hash: {vnpSecureHash}");
            _logger.LogDebug($"Payment URL: {paymentUrl}");

            return paymentUrl;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating VNPay payment URL for order {orderCode}");
            throw;
        }
    }

    public bool ValidateSignature(Dictionary<string, string> responseData)
    {
        try
        {
            var hashSecret = _configuration["VNPay:HashSecret"];
            
            if (!responseData.ContainsKey("vnp_SecureHash"))
            {
                _logger.LogWarning("VNPay response missing vnp_SecureHash");
                return false;
            }

            var vnpSecureHash = responseData["vnp_SecureHash"];
            
            // Remove hash from data
            var sortedData = new SortedDictionary<string, string>(responseData);
            sortedData.Remove("vnp_SecureHash");
            sortedData.Remove("vnp_SecureHashType");

            // Build sign data
            var signDataBuilder = new StringBuilder();
            foreach (var kvp in sortedData)
            {
                if (!string.IsNullOrEmpty(kvp.Value))
                {
                    signDataBuilder.Append($"{kvp.Key}={kvp.Value}&");
                }
            }

            var signData = signDataBuilder.ToString().TrimEnd('&');
            var checkSum = HmacSHA256(hashSecret ?? "", signData);

            var isValid = checkSum.Equals(vnpSecureHash, StringComparison.InvariantCultureIgnoreCase);

            if (!isValid)
            {
                _logger.LogWarning($"VNPay signature validation failed. Expected: {checkSum}, Got: {vnpSecureHash}");
            }

            return isValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating VNPay signature");
            return false;
        }
    }

    public Dictionary<string, string> ParseQueryString(string queryString)
    {
        var result = new Dictionary<string, string>();
        
        if (string.IsNullOrEmpty(queryString))
            return result;

        var pairs = queryString.TrimStart('?').Split('&');
        
        foreach (var pair in pairs)
        {
            var parts = pair.Split('=');
            if (parts.Length == 2)
            {
                var key = HttpUtility.UrlDecode(parts[0]);
                var value = HttpUtility.UrlDecode(parts[1]);
                result[key] = value;
            }
        }

        return result;
    }

    private string HmacSHA256(string key, string data)
    {
        var keyBytes = Encoding.UTF8.GetBytes(key);
        var dataBytes = Encoding.UTF8.GetBytes(data);

        using (var hmac = new HMACSHA256(keyBytes))
        {
            var hashBytes = hmac.ComputeHash(dataBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }

    private string NormalizeIpAddress(string ipAddress)
    {
        if (string.IsNullOrEmpty(ipAddress))
            return "127.0.0.1";

        // Convert IPv6 localhost to IPv4
        if (ipAddress == "::1" || ipAddress == "0:0:0:0:0:0:0:1")
            return "127.0.0.1";

        // Remove IPv6 brackets if present
        if (ipAddress.StartsWith("[") && ipAddress.EndsWith("]"))
            ipAddress = ipAddress.Substring(1, ipAddress.Length - 2);

        // If it's still IPv6, try to extract IPv4 from mapped IPv4
        if (ipAddress.Contains("::ffff:"))
        {
            var parts = ipAddress.Split(':');
            if (parts.Length > 0)
            {
                var lastPart = parts[parts.Length - 1];
                if (System.Net.IPAddress.TryParse(lastPart, out var ip))
                {
                    return lastPart;
                }
            }
        }

        // Return as-is if it's already a valid IPv4
        if (System.Net.IPAddress.TryParse(ipAddress, out var parsedIp))
        {
            return parsedIp.ToString();
        }

        // Fallback to localhost
        return "127.0.0.1";
    }
}

