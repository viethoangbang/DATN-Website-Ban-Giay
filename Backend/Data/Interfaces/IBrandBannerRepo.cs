using Data.Models;

namespace Data.Interfaces;

public interface IBrandBannerRepo : IRepository<BrandBanner>
{
    Task<IEnumerable<BrandBanner>> GetByBrandAsync(string brand);
    Task<IEnumerable<BrandBanner>> GetActiveByBrandAsync(string brand);
    Task<IEnumerable<string>> GetDistinctBrandsAsync();
}
