using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Business.Services;

public interface IGHNLocationService
{
    Task<List<GHNProvince>> GetProvincesAsync();
    Task<List<GHNDistrict>> GetDistrictsAsync(int provinceId);
    Task<List<GHNWard>> GetWardsAsync(int districtId);
}

public class GHNLocationService : IGHNLocationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<GHNLocationService> _logger;
    private readonly string _baseUrl;
    private readonly string _token;

    public GHNLocationService(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<GHNLocationService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
        
        _baseUrl = _configuration["GHN:BaseUrl"] ?? "https://dev-online-gateway.ghn.vn/shiip/public-api";
        _token = _configuration["GHN:Token"] ?? "";
    }

    public async Task<List<GHNProvince>> GetProvincesAsync()
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("Token", _token);

            var url = $"{_baseUrl}/master-data/province";
            
            var response = await httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"GHN provinces API error: {response.StatusCode}");
                return new List<GHNProvince>();
            }

            var jsonDoc = JsonDocument.Parse(content);
            var root = jsonDoc.RootElement;

            if (root.TryGetProperty("code", out var code) && code.GetInt32() == 200)
            {
                if (root.TryGetProperty("data", out var data) && data.ValueKind == JsonValueKind.Array)
                {
                    var provinces = new List<GHNProvince>();
                    
                    foreach (var item in data.EnumerateArray())
                    {
                        provinces.Add(new GHNProvince
                        {
                            ProvinceID = item.TryGetProperty("ProvinceID", out var pid) ? pid.GetInt32() : 0,
                            ProvinceName = item.TryGetProperty("ProvinceName", out var pname) ? pname.GetString() : "",
                            Code = item.TryGetProperty("Code", out var pcode) ? pcode.GetString() : ""
                        });
                    }
                    
                    return provinces;
                }
            }

            return new List<GHNProvince>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching GHN provinces");
            return new List<GHNProvince>();
        }
    }

    public async Task<List<GHNDistrict>> GetDistrictsAsync(int provinceId)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("Token", _token);

            var url = $"{_baseUrl}/master-data/district?province_id={provinceId}";
            
            var response = await httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"GHN districts API error: {response.StatusCode}");
                return new List<GHNDistrict>();
            }

            var jsonDoc = JsonDocument.Parse(content);
            var root = jsonDoc.RootElement;

            if (root.TryGetProperty("code", out var code) && code.GetInt32() == 200)
            {
                if (root.TryGetProperty("data", out var data) && data.ValueKind == JsonValueKind.Array)
                {
                    var districts = new List<GHNDistrict>();
                    
                    foreach (var item in data.EnumerateArray())
                    {
                        districts.Add(new GHNDistrict
                        {
                            DistrictID = item.TryGetProperty("DistrictID", out var did) ? did.GetInt32() : 0,
                            DistrictName = item.TryGetProperty("DistrictName", out var dname) ? dname.GetString() : "",
                            ProvinceID = item.TryGetProperty("ProvinceID", out var pid) ? pid.GetInt32() : 0
                        });
                    }
                    
                    return districts;
                }
            }

            return new List<GHNDistrict>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching GHN districts for province {provinceId}");
            return new List<GHNDistrict>();
        }
    }

    public async Task<List<GHNWard>> GetWardsAsync(int districtId)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("Token", _token);

            var url = $"{_baseUrl}/master-data/ward?district_id={districtId}";
            
            var response = await httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"GHN wards API error: {response.StatusCode}");
                return new List<GHNWard>();
            }

            var jsonDoc = JsonDocument.Parse(content);
            var root = jsonDoc.RootElement;

            if (root.TryGetProperty("code", out var code) && code.GetInt32() == 200)
            {
                if (root.TryGetProperty("data", out var data) && data.ValueKind == JsonValueKind.Array)
                {
                    var wards = new List<GHNWard>();
                    
                    foreach (var item in data.EnumerateArray())
                    {
                        wards.Add(new GHNWard
                        {
                            WardCode = item.TryGetProperty("WardCode", out var wcode) ? wcode.GetString() : "",
                            WardName = item.TryGetProperty("WardName", out var wname) ? wname.GetString() : "",
                            DistrictID = item.TryGetProperty("DistrictID", out var did) ? did.GetInt32() : 0
                        });
                    }
                    
                    return wards;
                }
            }

            return new List<GHNWard>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching GHN wards for district {districtId}");
            return new List<GHNWard>();
        }
    }
}

// GHN Data Models
public class GHNProvince
{
    public int ProvinceID { get; set; }
    public string? ProvinceName { get; set; }
    public string? Code { get; set; }
}

public class GHNDistrict
{
    public int DistrictID { get; set; }
    public string? DistrictName { get; set; }
    public int ProvinceID { get; set; }
}

public class GHNWard
{
    public string? WardCode { get; set; }
    public string? WardName { get; set; }
    public int DistrictID { get; set; }
}

