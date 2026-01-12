using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class DiscountRepo : Repository<Discount>, IDiscountRepo
{
    public DiscountRepo(SneakerShopContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Discount>> GetByProductIdAsync(int productId)
    {
        return await _dbSet
            .Where(d => d.ProductId == productId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Discount>> GetActiveByProductIdAsync(int productId)
    {
        var now = DateTime.Now;
        return await _dbSet
            .Where(d => d.ProductId == productId 
                && d.Status == "Active"
                && d.StartDate <= now
                && d.EndDate >= now)
            .ToListAsync();
    }
}
