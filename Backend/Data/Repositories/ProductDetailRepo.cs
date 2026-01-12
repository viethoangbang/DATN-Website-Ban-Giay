using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ProductDetailRepo : Repository<ProductDetail>, IProductDetailRepo
{
    public ProductDetailRepo(SneakerShopContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<ProductDetail>> GetAllAsync()
    {
        return await _dbSet
            .Include(pd => pd.Image)
            .Include(pd => pd.ProductDetailImages)
                .ThenInclude(pdi => pdi.Image)
            .Include(pd => pd.Color)
            .Include(pd => pd.Size)
            .ToListAsync();
    }

    public override async Task<ProductDetail?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(pd => pd.Image)
            .Include(pd => pd.ProductDetailImages)
                .ThenInclude(pdi => pdi.Image)
            .Include(pd => pd.Color)
            .Include(pd => pd.Size)
            .FirstOrDefaultAsync(pd => pd.Id == id);
    }

    public async Task<IEnumerable<ProductDetail>> GetByProductIdAsync(int productId)
    {
        return await _dbSet
            .Where(pd => pd.ProductId == productId)
            .Include(pd => pd.Image)
            .Include(pd => pd.ProductDetailImages)
                .ThenInclude(pdi => pdi.Image)
            .Include(pd => pd.Color)
            .Include(pd => pd.Size)
            .ToListAsync();
    }

    public async Task<ProductDetail?> GetByVariantAsync(int productId, int colorId, int sizeId, int? excludeId = null)
    {
        var query = _dbSet
            .Where(pd => pd.ProductId == productId 
                && pd.ColorId == colorId 
                && pd.SizeId == sizeId);
        
        if (excludeId.HasValue)
        {
            query = query.Where(pd => pd.Id != excludeId.Value);
        }
        
        return await query.FirstOrDefaultAsync();
    }
}
