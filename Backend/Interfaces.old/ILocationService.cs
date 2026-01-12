namespace Business.Interfaces;

public interface ILocationService
{
    Task<object> GetProvincesAsync();
    Task<object> GetDistrictsAsync(string provinceCode);
    Task<object> GetWardsAsync(string districtCode);
}

