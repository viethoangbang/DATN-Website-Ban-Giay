using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class BrandRepo : Repository<Brand>, IBrandRepo
{
    public BrandRepo(SneakerShopContext context) : base(context)
    {
    }

    public async Task<Brand?> GetByNameAsync(string name)
    {
        return await _dbSet
            .FirstOrDefaultAsync(b => b.Name == name);
    }

    public async Task<IEnumerable<Brand>> GetActiveBrandsAsync()
    {
        return await _dbSet
            .Where(b => b.Status == "Active")
            .OrderBy(b => b.DisplayOrder ?? 0)
            .ThenBy(b => b.Name)
            .ToListAsync();
    }
}
