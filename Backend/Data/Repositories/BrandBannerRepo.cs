using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class BrandBannerRepo : Repository<BrandBanner>, IBrandBannerRepo
{
    public BrandBannerRepo(SneakerShopContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<BrandBanner>> GetAllAsync()
    {
        return await _dbSet
            .Include(bb => bb.BrandNavigation)
            .Include(bb => bb.Image)
            .ToListAsync();
    }

    public override async Task<BrandBanner?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(bb => bb.BrandNavigation)
            .Include(bb => bb.Image)
            .FirstOrDefaultAsync(bb => bb.Id == id);
    }

    public async Task<IEnumerable<BrandBanner>> GetByBrandAsync(string brand)
    {
        return await _dbSet
            .Include(bb => bb.BrandNavigation)
            .Include(bb => bb.Image)
            .Where(bb => bb.Brand == brand || (bb.BrandNavigation != null && bb.BrandNavigation.Name == brand))
            .ToListAsync();
    }

    public async Task<IEnumerable<BrandBanner>> GetActiveByBrandAsync(string brand)
    {
        return await _dbSet
            .Include(bb => bb.BrandNavigation)
            .Include(bb => bb.Image)
            .Where(bb => (bb.Brand == brand || (bb.BrandNavigation != null && bb.BrandNavigation.Name == brand)) && bb.Status == "Active")
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> GetDistinctBrandsAsync()
    {
        // Get brands from both BrandNavigation and Brand string field
        var brandsFromNavigation = await _dbSet
            .Where(bb => bb.BrandNavigation != null && !string.IsNullOrEmpty(bb.BrandNavigation!.Name))
            .Select(bb => bb.BrandNavigation!.Name!)
            .Distinct()
            .ToListAsync();
        
        var brandsFromString = await _dbSet
            .Where(bb => bb.Brand != null && !string.IsNullOrEmpty(bb.Brand))
            .Select(bb => bb.Brand!)
            .Distinct()
            .ToListAsync();
        
        return brandsFromNavigation.Union(brandsFromString).Distinct().ToList();
    }
}
