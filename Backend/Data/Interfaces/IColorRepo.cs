using Data.Models;

namespace Data.Interfaces;

public interface IColorRepo : IRepository<Color>
{
    Task<Color?> GetByNameAsync(string name, int? excludeId = null);
}
