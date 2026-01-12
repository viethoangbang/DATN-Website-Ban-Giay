using Business.Interfaces;
using Business.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Business.Services;

public class LocationService : ILocationService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<LocationService> _logger;
    private readonly string _baseUrl;
    private readonly string _apiKey;

    public LocationService(IConfiguration configuration, ILogger<LocationService> logger, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _logger = logger;
        
        _baseUrl = _configuration["LocationApi:BaseUrl"] ?? "https://www.tinhthanhpho.com/api/v1";
        _apiKey = _configuration["LocationApi:ApiKey"] ?? string.Empty;

        if (string.IsNullOrEmpty(_apiKey))
        {
            _logger.LogWarning("LocationApi:ApiKey is not configured in appsettings.json");
        }

        _httpClient = httpClientFactory.CreateClient();
        // Don't set BaseAddress, use full URLs instead
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.Timeout = TimeSpan.FromSeconds(30); // Increase timeout
    }

    public async Task<object> GetProvincesAsync()
    {
        try
        {
            var url = $"{_baseUrl}/provinces";
            _logger.LogInformation("Fetching provinces from Location API: {Url}", url);
            var response = await _httpClient.GetAsync(url);
            
            List<object> apiProvinces = new List<object>();
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Location API response received: {ContentLength} bytes", content.Length);
                
                // Try to parse as array or object with data property
                try
                {
                    var jsonNode = JsonNode.Parse(content);
                    if (jsonNode is JsonArray jsonArray)
                    {
                        apiProvinces = jsonArray.Cast<object>().ToList();
                    }
                    else if (jsonNode is JsonObject jsonObject)
                    {
                        if (jsonObject["data"] is JsonArray dataArray)
                        {
                            apiProvinces = dataArray.Cast<object>().ToList();
                        }
                        else if (jsonObject["provinces"] is JsonArray provincesArray)
                        {
                            apiProvinces = provincesArray.Cast<object>().ToList();
                        }
                        else if (jsonObject["results"] is JsonArray resultsArray)
                        {
                            apiProvinces = resultsArray.Cast<object>().ToList();
                        }
                    }
                }
                catch (JsonException ex)
                {
                    _logger.LogWarning(ex, "Failed to parse API response as JSON, using fallback list");
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Location API returned error: {StatusCode} - {ErrorContent}, using fallback list", response.StatusCode, errorContent);
            }
            
            // Get fallback list
            var fallbackProvinces = VietnamProvinces.GetAllProvinces();
            
            // Merge API data with fallback list
            var mergedProvinces = new List<object>();
            
            // First, add all provinces from API
            foreach (var apiProvince in apiProvinces)
            {
                mergedProvinces.Add(apiProvince);
            }
            
            // Then, add any missing provinces from fallback list
            foreach (var fallbackProvince in fallbackProvinces)
            {
                // Check if province already exists in API data (by name or code)
                bool exists = false;
                foreach (var apiProv in apiProvinces)
                {
                    if (apiProv is JsonObject apiObj)
                    {
                        var name = apiObj["name"]?.ToString() ?? "";
                        var code = apiObj["code"]?.ToString() ?? "";
                        if (name.Equals(fallbackProvince.Name, StringComparison.OrdinalIgnoreCase) ||
                            code.Equals(fallbackProvince.Code, StringComparison.OrdinalIgnoreCase))
                        {
                            exists = true;
                            break;
                        }
                    }
                }
                
                if (!exists)
                {
                    mergedProvinces.Add(new
                    {
                        code = fallbackProvince.Code,
                        name = fallbackProvince.Name
                    });
                }
            }
            
            _logger.LogInformation("Successfully merged provinces: {ApiCount} from API, {TotalCount} total", apiProvinces.Count, mergedProvinces.Count);
            return mergedProvinces;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "HTTP error fetching provinces from Location API, using fallback list");
            // Return fallback list if API fails
            var fallbackProvinces = VietnamProvinces.GetAllProvinces();
            return fallbackProvinces.Select(p => new { code = p.Code, name = p.Name }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error fetching provinces from Location API: {Message}, using fallback list", ex.Message);
            // Return fallback list if any error occurs
            var fallbackProvinces = VietnamProvinces.GetAllProvinces();
            return fallbackProvinces.Select(p => new { code = p.Code, name = p.Name }).ToList();
        }
    }

    public async Task<object> GetDistrictsAsync(string provinceCode)
    {
        try
        {
            if (string.IsNullOrEmpty(provinceCode))
            {
                throw new ArgumentException("Province code cannot be empty", nameof(provinceCode));
            }

            var url = $"{_baseUrl}/provinces/{provinceCode}/districts";
            _logger.LogInformation("Fetching districts for province: {ProvinceCode} from {Url}", provinceCode, url);
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<object>(content);
            
            _logger.LogInformation("Successfully fetched districts for province: {ProvinceCode}", provinceCode);
            return result ?? new { };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching districts for province: {ProvinceCode}", provinceCode);
            throw new Exception($"Error fetching districts: {ex.Message}", ex);
        }
    }

    public async Task<object> GetWardsAsync(string districtCode)
    {
        try
        {
            if (string.IsNullOrEmpty(districtCode))
            {
                throw new ArgumentException("District code cannot be empty", nameof(districtCode));
            }

            var url = $"{_baseUrl}/districts/{districtCode}/wards";
            _logger.LogInformation("Fetching wards for district: {DistrictCode} from {Url}", districtCode, url);
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<object>(content);
            
            _logger.LogInformation("Successfully fetched wards for district: {DistrictCode}", districtCode);
            return result ?? new { };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching wards for district: {DistrictCode}", districtCode);
            throw new Exception($"Error fetching wards: {ex.Message}", ex);
        }
    }
}

