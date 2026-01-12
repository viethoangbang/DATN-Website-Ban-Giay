using Data.Models;

namespace Data.Interfaces;

public interface IVoucherRepo : IRepository<Voucher>
{
    Task<Voucher?> GetByCodeAsync(string code);
}
