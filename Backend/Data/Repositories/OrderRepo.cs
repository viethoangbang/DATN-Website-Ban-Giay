using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories;

public class OrderRepo : Repository<Order>, IOrderRepo
{
    public OrderRepo(SneakerShopContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Order>> GetByCustomerIdAsync(int customerId)
    {
        return await _dbSet
            .Where(o => o.CustomerId == customerId)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.ProductDetail)
                    .ThenInclude(pd => pd!.Product)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.ProductDetail)
                    .ThenInclude(pd => pd!.Color)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.ProductDetail)
                    .ThenInclude(pd => pd!.Size)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.ProductDetail)
                    .ThenInclude(pd => pd!.Image)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.ProductDetail)
                    .ThenInclude(pd => pd!.ProductDetailImages)
                        .ThenInclude(pdi => pdi.Image)
            .Include(o => o.OrderStatusHistories)
            .OrderByDescending(o => o.CreateDate)
            .ToListAsync();
    }

    public async Task<Order?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.ProductDetail)
                    .ThenInclude(pd => pd!.Product)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.ProductDetail)
                    .ThenInclude(pd => pd!.Color)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.ProductDetail)
                    .ThenInclude(pd => pd!.Size)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.ProductDetail)
                    .ThenInclude(pd => pd!.Image)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.ProductDetail)
                    .ThenInclude(pd => pd!.ProductDetailImages)
                        .ThenInclude(pdi => pdi.Image)
            .Include(o => o.OrderStatusHistories)
            .FirstOrDefaultAsync(o => o.Id == id);
    }
}
