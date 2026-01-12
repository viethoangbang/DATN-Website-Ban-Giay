using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ColorRepo : Repository<Color>, IColorRepo
{
    public ColorRepo(SneakerShopContext context) : base(context)
    {
    }

    public async Task<Color?> GetByNameAsync(string name, int? excludeId = null)
    {
        var query = _dbSet.Where(c => c.Name == name);
        
        if (excludeId.HasValue)
        {
            query = query.Where(c => c.Id != excludeId.Value);
        }
        
        return await query.FirstOrDefaultAsync();
    }
}
