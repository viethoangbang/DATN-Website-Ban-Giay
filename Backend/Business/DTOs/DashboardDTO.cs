namespace Business.DTOs;

public class DashboardStatsDto
{
    public decimal TotalRevenue { get; set; }
    public decimal RevenueChangePercent { get; set; }
    public int TotalOrders { get; set; }
    public decimal OrdersChangePercent { get; set; }
    public int TotalCustomers { get; set; }
    public decimal CustomersChangePercent { get; set; }
    public int TotalProducts { get; set; }
    public decimal ProductsChangePercent { get; set; }
}

public class RecentOrderDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; }
}

public class TopProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Sold { get; set; }
    public decimal Revenue { get; set; }
    public string? ImageUrl { get; set; }
}

public class MonthlyRevenueDto
{
    public string Month { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
}

public class CategoryRevenueDto
{
    public string CategoryName { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public decimal Percentage { get; set; }
}

public class DashboardResponseDto
{
    public DashboardStatsDto Stats { get; set; } = new();
    public List<RecentOrderDto> RecentOrders { get; set; } = new();
    public List<TopProductDto> TopProducts { get; set; } = new();
    public List<MonthlyRevenueDto> MonthlyRevenue { get; set; } = new();
    public List<CategoryRevenueDto> CategoryRevenue { get; set; } = new();
}

