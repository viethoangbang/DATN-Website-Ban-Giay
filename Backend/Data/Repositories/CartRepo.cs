using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class CartRepo : Repository<Cart>, ICartRepo
{
    public CartRepo(SneakerShopContext context) : base(context)
    {
    }

    public async Task<Cart?> GetActiveCartByAccountIdAsync(int accountId)
    {
        return await _dbSet
            .Include(c => c.CartDetails)
                .ThenInclude(cd => cd.ProductDetail)
                    .ThenInclude(pd => pd.Size)
            .Include(c => c.CartDetails)
                .ThenInclude(cd => cd.ProductDetail)
                    .ThenInclude(pd => pd.Color)
            .Include(c => c.CartDetails)
                .ThenInclude(cd => cd.ProductDetail)
                    .ThenInclude(pd => pd.Image)
            .Include(c => c.CartDetails)
                .ThenInclude(cd => cd.ProductDetail)
                    .ThenInclude(pd => pd.Product)
            .FirstOrDefaultAsync(c => c.AccountId == accountId);
    }
}
