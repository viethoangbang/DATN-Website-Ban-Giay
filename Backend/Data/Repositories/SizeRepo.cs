using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class SizeRepo : Repository<Size>, ISizeRepo
{
    public SizeRepo(SneakerShopContext context) : base(context)
    {
    }

    public async Task<Size?> GetBySizeNameAsync(string sizeName, int? excludeId = null)
    {
        var query = _dbSet.Where(s => s.SizeName == sizeName);
        
        if (excludeId.HasValue)
        {
            query = query.Where(s => s.Id != excludeId.Value);
        }
        
        return await query.FirstOrDefaultAsync();
    }
}
