using Data.Models;

namespace Data.Interfaces;

public interface IAddressRepo : IRepository<Address>
{
    Task<IEnumerable<Address>> GetByAccountIdAsync(int accountId);
}
