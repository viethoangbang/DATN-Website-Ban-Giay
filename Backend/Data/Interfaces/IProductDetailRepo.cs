using Data.Models;

namespace Data.Interfaces;

public interface IProductDetailRepo : IRepository<ProductDetail>
{
    Task<IEnumerable<ProductDetail>> GetByProductIdAsync(int productId);
    Task<ProductDetail?> GetByVariantAsync(int productId, int colorId, int sizeId, int? excludeId = null);
}
