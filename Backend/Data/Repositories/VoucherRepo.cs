using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class VoucherRepo : Repository<Voucher>, IVoucherRepo
{
    public VoucherRepo(SneakerShopContext context) : base(context)
    {
    }

    public async Task<Voucher?> GetByCodeAsync(string code)
    {
        return await _dbSet
            .FirstOrDefaultAsync(v => v.Code == code);
    }
}
