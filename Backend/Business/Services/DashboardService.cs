using Business.DTOs;
using Business.Interfaces;
using Data.Models;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public class DashboardService : IDashboardService
{
    private readonly SneakerShopContext _context;

    public DashboardService(SneakerShopContext context)
    {
        _context = context;
    }

    public async Task<DashboardResponseDto> GetDashboardDataAsync()
    {
        try
        {
            var now = DateTime.Now;
            var currentMonth = new DateTime(now.Year, now.Month, 1);
        var lastMonth = currentMonth.AddMonths(-1);
        var lastMonthEnd = currentMonth.AddDays(-1);

        // Stats
        var currentMonthRevenue = await _context.Orders
            .Where(o => o.CreateDate >= currentMonth && o.Status != "Đã hủy")
            .SumAsync(o => o.Total ?? 0);

        var lastMonthRevenue = await _context.Orders
            .Where(o => o.CreateDate >= lastMonth && o.CreateDate < currentMonth && o.Status != "Đã hủy")
            .SumAsync(o => o.Total ?? 0);

        var revenueChangePercent = lastMonthRevenue > 0
            ? ((currentMonthRevenue - lastMonthRevenue) / lastMonthRevenue) * 100
            : 0;

        var currentMonthOrders = await _context.Orders
            .Where(o => o.CreateDate >= currentMonth)
            .CountAsync();

        var lastMonthOrders = await _context.Orders
            .Where(o => o.CreateDate >= lastMonth && o.CreateDate < currentMonth)
            .CountAsync();

        var ordersChangePercent = lastMonthOrders > 0
            ? ((currentMonthOrders - lastMonthOrders) / (double)lastMonthOrders) * 100
            : 0;

        var currentMonthCustomers = await _context.Accounts
            .Where(a => a.AccountRoles.Any(ar => ar.Role!.Name == "customer") 
                && a.CreateDate >= currentMonth)
            .CountAsync();

        var lastMonthCustomers = await _context.Accounts
            .Where(a => a.AccountRoles.Any(ar => ar.Role!.Name == "customer")
                && a.CreateDate >= lastMonth && a.CreateDate < currentMonth)
            .CountAsync();

        var customersChangePercent = lastMonthCustomers > 0
            ? ((currentMonthCustomers - lastMonthCustomers) / (double)lastMonthCustomers) * 100
            : 0;

        var currentMonthProducts = await _context.Products
            .Where(p => p.CreateDate >= currentMonth)
            .CountAsync();

        var lastMonthProducts = await _context.Products
            .Where(p => p.CreateDate >= lastMonth && p.CreateDate < currentMonth)
            .CountAsync();

        var productsChangePercent = lastMonthProducts > 0
            ? ((currentMonthProducts - lastMonthProducts) / (double)lastMonthProducts) * 100
            : 0;

        var stats = new DashboardStatsDto
        {
            TotalRevenue = currentMonthRevenue,
            RevenueChangePercent = (decimal)Math.Round(revenueChangePercent, 1),
            TotalOrders = currentMonthOrders,
            OrdersChangePercent = (decimal)Math.Round(ordersChangePercent, 1),
            TotalCustomers = await _context.Accounts
                .Where(a => a.AccountRoles.Any(ar => ar.Role!.Name == "customer"))
                .CountAsync(),
            CustomersChangePercent = (decimal)Math.Round(customersChangePercent, 1),
            TotalProducts = await _context.Products.CountAsync(),
            ProductsChangePercent = (decimal)Math.Round(productsChangePercent, 1)
        };

        // Recent Orders
        var recentOrders = await _context.Orders
            .Include(o => o.Customer)
            .OrderByDescending(o => o.CreateDate)
            .Take(5)
            .Select(o => new RecentOrderDto
            {
                Id = o.Id,
                Code = o.Code ?? "",
                CustomerName = o.Customer!.FullName ?? "Khách vãng lai",
                Total = o.Total ?? 0,
                Status = o.Status ?? "Chưa xác định",
                CreateDate = o.CreateDate ?? DateTime.Now
            })
            .ToListAsync();

        // Top Products (by revenue from order details)
        var topProductsData = await _context.OrderDetails
            .Include(od => od.ProductDetail)
                .ThenInclude(pd => pd != null ? pd.Product : null!)
            .Include(od => od.ProductDetail)
                .ThenInclude(pd => pd != null ? pd.ProductDetailImages : null!)
                    .ThenInclude(pdi => pdi != null ? pdi.Image : null!)
            .Include(od => od.Order)
            .Where(od => od.Order != null && od.Order.Status != "Đã hủy" 
                && od.ProductDetail != null 
                && od.ProductDetail.Product != null)
            .ToListAsync();

        var topProducts = topProductsData
            .Where(od => od.ProductDetail != null && od.ProductDetail.Product != null)
            .GroupBy(od => od.ProductDetail!.ProductId)
            .Select(g => 
            {
                var firstItem = g.First();
                var productDetail = firstItem.ProductDetail;
                var product = productDetail?.Product;
                
                return new TopProductDto
                {
                    Id = g.Key ?? 0,
                    Name = product?.Name ?? "",
                    Sold = g.Sum(od => od.Quantity ?? 0),
                    Revenue = g.Sum(od => (od.Quantity ?? 0) * (od.Price ?? 0)),
                    ImageUrl = productDetail?.ProductDetailImages?
                        .Where(pdi => pdi != null && pdi.Image != null)
                        .OrderBy(pdi => pdi.DisplayOrder ?? 0)
                        .FirstOrDefault()?.Image?.Url
                };
            })
            .OrderByDescending(p => p.Revenue)
            .Take(5)
            .ToList();

        // Monthly Revenue (last 6 months)
        var sixMonthsAgo = now.AddMonths(-5);
        var monthlyRevenue = await _context.Orders
            .Where(o => o.CreateDate >= sixMonthsAgo && o.Status != "Đã hủy")
            .GroupBy(o => new { Year = o.CreateDate!.Value.Year, Month = o.CreateDate.Value.Month })
            .Select(g => new MonthlyRevenueDto
            {
                Month = $"{g.Key.Month}/{g.Key.Year}",
                Revenue = g.Sum(o => o.Total ?? 0)
            })
            .OrderBy(m => m.Month)
            .ToListAsync();

        // Category Revenue
        var categoryRevenueData = await _context.OrderDetails
            .Include(od => od.ProductDetail)
                .ThenInclude(pd => pd != null ? pd.Product : null!)
                    .ThenInclude(p => p != null ? p.Category : null!)
            .Include(od => od.Order)
            .Where(od => od.Order != null && od.Order.Status != "Đã hủy" 
                && od.ProductDetail != null 
                && od.ProductDetail.Product != null)
            .ToListAsync();

        var categoryRevenue = categoryRevenueData
            .Where(od => od.ProductDetail != null && od.ProductDetail.Product != null)
            .GroupBy(od => 
            {
                var category = od.ProductDetail!.Product!.Category;
                return category != null ? (category.Name ?? "Chưa phân loại") : "Chưa phân loại";
            })
            .Select(g => new CategoryRevenueDto
            {
                CategoryName = g.Key ?? "Chưa phân loại",
                Revenue = g.Sum(od => (od.Quantity ?? 0) * (od.Price ?? 0))
            })
            .OrderByDescending(c => c.Revenue)
            .ToList();

        var totalCategoryRevenue = categoryRevenue.Sum(c => c.Revenue);
        foreach (var cat in categoryRevenue)
        {
            cat.Percentage = totalCategoryRevenue > 0
                ? (decimal)Math.Round((double)(cat.Revenue / totalCategoryRevenue) * 100, 1)
                : 0;
        }

            return new DashboardResponseDto
            {
                Stats = stats,
                RecentOrders = recentOrders,
                TopProducts = topProducts,
                MonthlyRevenue = monthlyRevenue,
                CategoryRevenue = categoryRevenue
            };
        }
        catch (Exception ex)
        {
            // Log error and return empty data instead of throwing
            // This prevents 500 error if database is empty or has issues
            return new DashboardResponseDto
            {
                Stats = new DashboardStatsDto(),
                RecentOrders = new List<RecentOrderDto>(),
                TopProducts = new List<TopProductDto>(),
                MonthlyRevenue = new List<MonthlyRevenueDto>(),
                CategoryRevenue = new List<CategoryRevenueDto>()
            };
        }
    }

    public async Task<DashboardResponseDto> GetReportsDataAsync(string period)
    {
        try
        {
            var now = DateTime.Now;
            DateTime startDate;
            DateTime endDate = now;

            // Determine date range based on period
            switch (period.ToLower())
            {
                case "week":
                    startDate = now.AddDays(-7);
                    break;
                case "month":
                    startDate = new DateTime(now.Year, now.Month, 1);
                    break;
                case "quarter":
                    var quarter = (now.Month - 1) / 3;
                    startDate = new DateTime(now.Year, quarter * 3 + 1, 1);
                    break;
                case "year":
                    startDate = new DateTime(now.Year, 1, 1);
                    break;
                default:
                    startDate = new DateTime(now.Year, now.Month, 1);
                    break;
            }

            // Get previous period for comparison
            var previousStartDate = startDate.AddDays(-(endDate - startDate).Days);
            var previousEndDate = startDate;

            // Stats for current period
            var currentRevenue = await _context.Orders
                .Where(o => o.CreateDate >= startDate && o.CreateDate <= endDate && o.Status != "Đã hủy" && o.Status != "Cancelled")
                .SumAsync(o => o.Total ?? 0);

            var previousRevenue = await _context.Orders
                .Where(o => o.CreateDate >= previousStartDate && o.CreateDate < previousEndDate && o.Status != "Đã hủy" && o.Status != "Cancelled")
                .SumAsync(o => o.Total ?? 0);

            var revenueChangePercent = previousRevenue > 0
                ? ((currentRevenue - previousRevenue) / previousRevenue) * 100
                : 0;

            var currentOrders = await _context.Orders
                .Where(o => o.CreateDate >= startDate && o.CreateDate <= endDate)
                .CountAsync();

            var previousOrders = await _context.Orders
                .Where(o => o.CreateDate >= previousStartDate && o.CreateDate < previousEndDate)
                .CountAsync();

            var ordersChangePercent = previousOrders > 0
                ? ((currentOrders - previousOrders) / (double)previousOrders) * 100
                : 0;

            var currentCustomers = await _context.Accounts
                .Where(a => a.AccountRoles.Any(ar => ar.Role!.Name == "customer")
                    && a.CreateDate >= startDate && a.CreateDate <= endDate)
                .CountAsync();

            var previousCustomers = await _context.Accounts
                .Where(a => a.AccountRoles.Any(ar => ar.Role!.Name == "customer")
                    && a.CreateDate >= previousStartDate && a.CreateDate < previousEndDate)
                .CountAsync();

            var customersChangePercent = previousCustomers > 0
                ? ((currentCustomers - previousCustomers) / (double)previousCustomers) * 100
                : 0;

            var stats = new DashboardStatsDto
            {
                TotalRevenue = currentRevenue,
                RevenueChangePercent = (decimal)Math.Round(revenueChangePercent, 1),
                TotalOrders = currentOrders,
                OrdersChangePercent = (decimal)Math.Round(ordersChangePercent, 1),
                TotalCustomers = await _context.Accounts
                    .Where(a => a.AccountRoles.Any(ar => ar.Role!.Name == "customer"))
                    .CountAsync(),
                CustomersChangePercent = (decimal)Math.Round(customersChangePercent, 1),
                TotalProducts = await _context.Products.CountAsync(),
                ProductsChangePercent = 0 // Can be calculated if needed
            };

            // Top Products for the period
            var topProductsData = await _context.OrderDetails
                .Include(od => od.ProductDetail)
                    .ThenInclude(pd => pd != null ? pd.Product : null!)
                .Include(od => od.ProductDetail)
                    .ThenInclude(pd => pd != null ? pd.ProductDetailImages : null!)
                        .ThenInclude(pdi => pdi != null ? pdi.Image : null!)
                .Include(od => od.Order)
                .Where(od => od.Order != null 
                    && od.Order.CreateDate >= startDate 
                    && od.Order.CreateDate <= endDate
                    && od.Order.Status != "Đã hủy" 
                    && od.Order.Status != "Cancelled"
                    && od.ProductDetail != null 
                    && od.ProductDetail.Product != null)
                .ToListAsync();

            var topProducts = topProductsData
                .Where(od => od.ProductDetail != null && od.ProductDetail.Product != null)
                .GroupBy(od => od.ProductDetail!.ProductId)
                .Select(g => 
                {
                    var firstItem = g.First();
                    var productDetail = firstItem.ProductDetail;
                    var product = productDetail?.Product;
                    
                    return new TopProductDto
                    {
                        Id = g.Key ?? 0,
                        Name = product?.Name ?? "",
                        Sold = g.Sum(od => od.Quantity ?? 0),
                        Revenue = g.Sum(od => (od.Quantity ?? 0) * (od.Price ?? 0)),
                        ImageUrl = productDetail?.ProductDetailImages?
                            .Where(pdi => pdi != null && pdi.Image != null)
                            .OrderBy(pdi => pdi.DisplayOrder ?? 0)
                            .FirstOrDefault()?.Image?.Url
                    };
                })
                .OrderByDescending(p => p.Revenue)
                .Take(10)
                .ToList();

            // Monthly Revenue - adjust based on period
            List<MonthlyRevenueDto> monthlyRevenue;
            if (period.ToLower() == "year")
            {
                // Show 12 months
                monthlyRevenue = await _context.Orders
                    .Where(o => o.CreateDate >= startDate && o.CreateDate <= endDate && o.Status != "Đã hủy" && o.Status != "Cancelled")
                    .GroupBy(o => new { Year = o.CreateDate!.Value.Year, Month = o.CreateDate.Value.Month })
                    .Select(g => new MonthlyRevenueDto
                    {
                        Month = $"{g.Key.Month}/{g.Key.Year}",
                        Revenue = g.Sum(o => o.Total ?? 0)
                    })
                    .OrderBy(m => m.Month)
                    .ToListAsync();
            }
            else
            {
                // Show daily or weekly data
                var days = (endDate - startDate).Days + 1;
                monthlyRevenue = await _context.Orders
                    .Where(o => o.CreateDate >= startDate && o.CreateDate <= endDate && o.Status != "Đã hủy" && o.Status != "Cancelled")
                    .GroupBy(o => o.CreateDate!.Value.Date)
                    .Select(g => new MonthlyRevenueDto
                    {
                        Month = g.Key.ToString("dd/MM"),
                        Revenue = g.Sum(o => o.Total ?? 0)
                    })
                    .OrderBy(m => m.Month)
                    .ToListAsync();
            }

            // Category Revenue
            var categoryRevenueData = await _context.OrderDetails
                .Include(od => od.ProductDetail)
                    .ThenInclude(pd => pd != null ? pd.Product : null!)
                        .ThenInclude(p => p != null ? p.Category : null!)
                .Include(od => od.Order)
                .Where(od => od.Order != null 
                    && od.Order.CreateDate >= startDate 
                    && od.Order.CreateDate <= endDate
                    && od.Order.Status != "Đã hủy" 
                    && od.Order.Status != "Cancelled"
                    && od.ProductDetail != null 
                    && od.ProductDetail.Product != null)
                .ToListAsync();

            var categoryRevenue = categoryRevenueData
                .Where(od => od.ProductDetail != null && od.ProductDetail.Product != null)
                .GroupBy(od => 
                {
                    var category = od.ProductDetail!.Product!.Category;
                    return category != null ? (category.Name ?? "Chưa phân loại") : "Chưa phân loại";
                })
                .Select(g => new CategoryRevenueDto
                {
                    CategoryName = g.Key ?? "Chưa phân loại",
                    Revenue = g.Sum(od => (od.Quantity ?? 0) * (od.Price ?? 0))
                })
                .OrderByDescending(c => c.Revenue)
                .ToList();

            var totalCategoryRevenue = categoryRevenue.Sum(c => c.Revenue);
            foreach (var cat in categoryRevenue)
            {
                cat.Percentage = totalCategoryRevenue > 0
                    ? (decimal)Math.Round((double)(cat.Revenue / totalCategoryRevenue) * 100, 1)
                    : 0;
            }

            return new DashboardResponseDto
            {
                Stats = stats,
                RecentOrders = new List<RecentOrderDto>(), // Not needed for reports
                TopProducts = topProducts,
                MonthlyRevenue = monthlyRevenue,
                CategoryRevenue = categoryRevenue
            };
        }
        catch (Exception)
        {
            return new DashboardResponseDto
            {
                Stats = new DashboardStatsDto(),
                RecentOrders = new List<RecentOrderDto>(),
                TopProducts = new List<TopProductDto>(),
                MonthlyRevenue = new List<MonthlyRevenueDto>(),
                CategoryRevenue = new List<CategoryRevenueDto>()
            };
        }
    }
}

