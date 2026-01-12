using Data.Models;

namespace Data.Interfaces;

public interface IBrandRepo : IRepository<Brand>
{
    Task<Brand?> GetByNameAsync(string name);
    Task<IEnumerable<Brand>> GetActiveBrandsAsync();
}
