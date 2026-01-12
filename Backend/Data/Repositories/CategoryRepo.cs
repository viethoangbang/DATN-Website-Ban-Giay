using Data.Interfaces;
using Data.Models;

namespace Data.Repositories;

public class CategoryRepo : Repository<Category>, ICategoryRepo
{
    public CategoryRepo(SneakerShopContext context) : base(context)
    {
    }
}
