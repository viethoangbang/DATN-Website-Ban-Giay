using Data.Models;

namespace Data.Interfaces;

public interface IImageRepo : IRepository<Image>
{
    Task<IEnumerable<Image>> GetByTypeAsync(string type);
    Task<IEnumerable<string>> GetDistinctTypesAsync();
}
