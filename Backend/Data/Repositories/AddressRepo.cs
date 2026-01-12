using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class AddressRepo : Repository<Address>, IAddressRepo
{
    public AddressRepo(SneakerShopContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Address>> GetByAccountIdAsync(int accountId)
    {
        return await _dbSet
            .Where(a => a.AccountId == accountId && a.Status == "Active")
            .ToListAsync();
    }
}
