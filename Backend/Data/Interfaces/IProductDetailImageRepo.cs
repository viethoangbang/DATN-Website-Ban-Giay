using Data.Models;

namespace Data.Interfaces;

public interface IProductDetailImageRepo : IRepository<ProductDetailImage>
{
    Task DeleteByProductDetailIdAsync(int productDetailId);
}
