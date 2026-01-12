using Data.Models;

namespace Data.Interfaces;

public interface IDiscountRepo : IRepository<Discount>
{
    Task<IEnumerable<Discount>> GetByProductIdAsync(int productId);
    Task<IEnumerable<Discount>> GetActiveByProductIdAsync(int productId);
}
