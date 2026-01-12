using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ProductRepo : Repository<Product>, IProductRepo
{
    public ProductRepo(SneakerShopContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _dbSet
            .Include(p => p.BrandNavigation)
            .Include(p => p.Category)
            .ToListAsync();
    }

    public override async Task<Product?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(p => p.BrandNavigation)
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}
