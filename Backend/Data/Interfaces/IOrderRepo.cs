using Data.Models;

namespace Data.Interfaces;

public interface IOrderRepo : IRepository<Order>
{
    Task<IEnumerable<Order>> GetByCustomerIdAsync(int customerId);
    Task<Order?> GetByIdWithDetailsAsync(int id);
}
