using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class CartDetailRepo : Repository<CartDetail>, ICartDetailRepo
{
    public CartDetailRepo(SneakerShopContext context) : base(context)
    {
    }

    public async Task<CartDetail?> GetByCartAndProductDetailAsync(int cartId, int productDetailId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(cd => cd.CartId == cartId && cd.ProductDetailId == productDetailId);
    }
}
