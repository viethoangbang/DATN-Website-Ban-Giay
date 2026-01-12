using Data.Models;

namespace Data.Interfaces;

public interface ICartDetailRepo : IRepository<CartDetail>
{
    Task<CartDetail?> GetByCartAndProductDetailAsync(int cartId, int productDetailId);
}
