using Business.DTOs;
using Business.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Business.Services;

public interface IGHNService
{
    Task<List<GHNShop>> GetShopsAsync();
    Task<ShippingFeeResponseDto> CalculateShippingFeeAsync(string toDistrictId, string toWardCode, int weight, int insuranceValue);
    Task<string> CreateShipmentAsync(int orderId);
    Task<string> GetShipmentStatusAsync(string orderCode);
}

public class GHNService : IGHNService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<GHNService> _logger;
    private readonly string _baseUrl;
    private readonly string _token;
    private readonly int _shopId;
    private readonly int _fromDistrictId;

    public GHNService(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<GHNService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
        
        _baseUrl = _configuration["GHN:BaseUrl"] ?? "https://dev-online-gateway.ghn.vn/shiip/public-api";
        _token = _configuration["GHN:Token"] ?? "";
        _shopId = int.TryParse(_configuration["GHN:ShopId"], out var sid) ? sid : 0;
        _fromDistrictId = int.TryParse(_configuration["GHN:FromDistrictId"], out var did) ? did : 0;
    }

    public async Task<List<GHNShop>> GetShopsAsync()
    {
        try
        {
            if (string.IsNullOrEmpty(_token))
            {
                _logger.LogWarning("GHN Token is not configured");
                return new List<GHNShop>();
            }

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("Token", _token);
            httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");

            var requestBody = new
            {
                offset = 0,
                limit = 50,
                client_phone = ""
            };

            var url = $"{_baseUrl}/v2/shop/all";
            var content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            _logger.LogInformation($"Calling GHN GetShops API: {url}");

            var response = await httpClient.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            _logger.LogInformation($"GHN GetShops response: {responseContent}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"GHN GetShops API error: {response.StatusCode} - {responseContent}");
                
                // Try to parse error message
                try
                {
                    var errorDoc = JsonDocument.Parse(responseContent);
                    var errorRoot = errorDoc.RootElement;
                    var errorMessage = errorRoot.TryGetProperty("message", out var msg) 
                        ? msg.GetString() 
                        : "Unknown error";
                    _logger.LogError($"GHN error message: {errorMessage}");
                }
                catch { }
                
                return new List<GHNShop>();
            }

            var jsonDoc = JsonDocument.Parse(responseContent);
            var root = jsonDoc.RootElement;

            // Check response code
            if (root.TryGetProperty("code", out var code))
            {
                var codeValue = code.GetInt32();
                if (codeValue != 200)
                {
                    var message = root.TryGetProperty("message", out var msg) ? msg.GetString() : "Unknown error";
                    _logger.LogError($"GHN API returned code {codeValue}: {message}");
                    return new List<GHNShop>();
                }
            }

            // Parse shops data
            if (root.TryGetProperty("data", out var data))
            {
                var shops = new List<GHNShop>();
                
                // Data might be array or object with shops array
                if (data.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in data.EnumerateArray())
                    {
                        shops.Add(ParseShopItem(item));
                    }
                }
                else if (data.ValueKind == JsonValueKind.Object)
                {
                    // Try to find shops array in object
                    if (data.TryGetProperty("shops", out var shopsArray) && shopsArray.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var item in shopsArray.EnumerateArray())
                        {
                            shops.Add(ParseShopItem(item));
                        }
                    }
                }
                
                _logger.LogInformation($"Parsed {shops.Count} shop(s) from GHN API");
                return shops;
            }

            _logger.LogWarning("GHN GetShops response does not contain data");
            return new List<GHNShop>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting GHN shops");
            return new List<GHNShop>();
        }
    }

    private GHNShop ParseShopItem(JsonElement item)
    {
        // GHN API might use _id or id or shop_id
        int shopId = 0;
        if (item.TryGetProperty("_id", out var id1))
        {
            shopId = id1.GetInt32();
        }
        else if (item.TryGetProperty("id", out var id2))
        {
            shopId = id2.GetInt32();
        }
        else if (item.TryGetProperty("shop_id", out var id3))
        {
            shopId = id3.GetInt32();
        }

        return new GHNShop
        {
            ShopId = shopId,
            Name = item.TryGetProperty("name", out var name) ? name.GetString() : "",
            Phone = item.TryGetProperty("phone", out var phone) ? phone.GetString() : ""
        };
    }

    public async Task<ShippingFeeResponseDto> CalculateShippingFeeAsync(
        string toDistrictId, 
        string toWardCode, 
        int weight, 
        int insuranceValue)
    {
        try
        {
            // Validate shop ID
            if (_shopId == 0)
            {
                _logger.LogWarning("ShopId is not configured, using default shipping fee");
                return GetDefaultShippingFee("ShopId not configured");
            }

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("Token", _token);
            httpClient.DefaultRequestHeaders.Add("ShopId", _shopId.ToString());
            // Note: Content-Type should be set on HttpContent, not DefaultRequestHeaders

            var toDistrictIdInt = int.TryParse(toDistrictId, out var districtId) ? districtId : 0;
            if (toDistrictIdInt == 0)
            {
                _logger.LogWarning($"Invalid district ID: {toDistrictId}, using default shipping fee");
                return GetDefaultShippingFee("Invalid district ID");
            }

            var requestBody = new
            {
                service_type_id = 2, // 2 = Standard (chậm nhất, rẻ nhất)
                from_district_id = _fromDistrictId,
                to_district_id = toDistrictIdInt,
                to_ward_code = toWardCode,
                height = 15, // cm
                length = 30, // cm
                weight = weight, // gram
                width = 20, // cm
                insurance_value = insuranceValue,
                coupon = (string?)null
            };

            var url = $"{_baseUrl}/v2/shipping-order/fee";
            var content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            _logger.LogInformation($"Calculating GHN shipping fee: ShopId={_shopId}, FromDistrict={_fromDistrictId}, ToDistrict={toDistrictIdInt}, ToWard={toWardCode}");

            var response = await httpClient.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            _logger.LogInformation($"GHN response: {responseContent}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"GHN API error: {response.StatusCode} - {responseContent}");
                
                // Parse error message
                string? errorMessage = null;
                try
                {
                    var errorDoc = JsonDocument.Parse(responseContent);
                    var errorRoot = errorDoc.RootElement;
                    errorMessage = errorRoot.TryGetProperty("message", out var msg) 
                        ? msg.GetString() 
                        : "GHN API error";
                    
                    _logger.LogError($"GHN error message: {errorMessage}");
                }
                catch { }
                
                // If shop info error, use default fee with informative message
                if (errorMessage != null && (errorMessage.Contains("shop") || errorMessage.Contains("Shop") || errorMessage.Contains("SHOP_INFO_ERROR")))
                {
                    _logger.LogWarning($"GHN shop info error. Using default shipping fee. Error: {errorMessage}");
                    return GetDefaultShippingFee($"Thông tin shop chưa được cấu hình đầy đủ trong GHN. Vui lòng cập nhật thông tin shop trong GHN dashboard hoặc liên hệ admin.");
                }
                
                return GetDefaultShippingFee($"GHN API error: {errorMessage ?? "Unknown error"}");
            }

            var jsonDoc = JsonDocument.Parse(responseContent);
            var root = jsonDoc.RootElement;

            if (root.TryGetProperty("code", out var code) && code.GetInt32() == 200)
            {
                if (root.TryGetProperty("data", out var data))
                {
                    var totalFee = data.TryGetProperty("total", out var total)
                        ? total.GetDecimal()
                        : 30000;

                    var serviceFee = data.TryGetProperty("service_fee", out var serviceFeeValue)
                        ? serviceFeeValue.GetDecimal()
                        : totalFee;

                    return new ShippingFeeResponseDto
                    {
                        Fee = totalFee,
                        ServiceName = "GHN Express",
                        EstimatedDays = 3,
                        Message = "Estimated delivery time"
                    };
                }
            }

            return GetDefaultShippingFee("Invalid response format");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating GHN shipping fee");
            return GetDefaultShippingFee($"Error: {ex.Message}");
        }
    }

    public async Task<string> CreateShipmentAsync(int orderId)
    {
        try
        {
            // For demo/sandbox, return mock order code
            // In production, call actual GHN create order API
            await Task.Delay(100);
            
            var orderCode = $"GHN{DateTime.Now:yyyyMMddHHmmss}{orderId}";
            
            _logger.LogInformation($"Created GHN shipment: {orderCode} for order {orderId}");
            
            return orderCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating GHN shipment for order {orderId}");
            throw;
        }
    }

    public async Task<string> GetShipmentStatusAsync(string orderCode)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("Token", _token);

            var requestBody = new
            {
                order_code = orderCode
            };

            var url = $"{_baseUrl}/v2/shipping-order/detail";
            var content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await httpClient.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"GHN status API error: {response.StatusCode}");
                return "Unknown";
            }

            var jsonDoc = JsonDocument.Parse(responseContent);
            var root = jsonDoc.RootElement;

            if (root.TryGetProperty("data", out var data))
            {
                if (data.TryGetProperty("status", out var status))
                {
                    return status.GetString() ?? "Unknown";
                }
            }

            return "Unknown";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting GHN shipment status for {orderCode}");
            return "Error";
        }
    }

    private ShippingFeeResponseDto GetDefaultShippingFee(string message)
    {
        return new ShippingFeeResponseDto
        {
            Fee = 30000,
            ServiceName = "Standard",
            EstimatedDays = 3,
            Message = $"Using default shipping fee ({message})"
        };
    }
}

// GHN Shop Model
public class GHNShop
{
    public int ShopId { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
}

