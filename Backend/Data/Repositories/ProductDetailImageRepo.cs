using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ProductDetailImageRepo : Repository<ProductDetailImage>, IProductDetailImageRepo
{
    public ProductDetailImageRepo(SneakerShopContext context) : base(context)
    {
    }

    public async Task DeleteByProductDetailIdAsync(int productDetailId)
    {
        var images = await _dbSet
            .Where(pdi => pdi.ProductDetailId == productDetailId)
            .ToListAsync();
        
        _dbSet.RemoveRange(images);
        await _context.SaveChangesAsync();
    }
}
