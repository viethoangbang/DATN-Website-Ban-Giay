using Data.Interfaces;
using Data.Models;

namespace Data.Repositories;

public class ShipmentRepo : Repository<Shipment>, IShipmentRepo
{
    public ShipmentRepo(SneakerShopContext context) : base(context)
    {
    }
}
