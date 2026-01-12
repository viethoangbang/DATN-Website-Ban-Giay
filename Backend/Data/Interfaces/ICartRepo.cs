using Data.Models;

namespace Data.Interfaces;

public interface ICartRepo : IRepository<Cart>
{
    Task<Cart?> GetActiveCartByAccountIdAsync(int accountId);
}
