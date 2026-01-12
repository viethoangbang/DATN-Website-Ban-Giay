using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ImageRepo : Repository<Image>, IImageRepo
{
    public ImageRepo(SneakerShopContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Image>> GetByTypeAsync(string type)
    {
        return await _dbSet
            .Where(img => img.Type == type)
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> GetDistinctTypesAsync()
    {
        return await _dbSet
            .Select(img => img.Type)
            .Distinct()
            .ToListAsync();
    }
}
