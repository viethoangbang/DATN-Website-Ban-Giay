using Data.Interfaces;
using Data.Models;

namespace Data.Repositories;

public class PaymentRepo : Repository<Payment>, IPaymentRepo
{
    public PaymentRepo(SneakerShopContext context) : base(context)
    {
    }
}
