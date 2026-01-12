using Data.Models;

namespace Data.Interfaces;

public interface ISizeRepo : IRepository<Size>
{
    Task<Size?> GetBySizeNameAsync(string sizeName, int? excludeId = null);
}
