using Data.Interfaces;
using Data.Models;

namespace Data.Repositories;

public class OrderDetailRepo : Repository<OrderDetail>, IOrderDetailRepo
{
    public OrderDetailRepo(SneakerShopContext context) : base(context)
    {
    }
}
