using Business.DTOs;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Roles = "admin,employee")]
[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _service;
    
    public DashboardController(IDashboardService service)
    {
        _service = service;
    }

    [HttpGet("stats")]
    public async Task<ActionResult<DashboardResponseDto>> GetDashboardStats()
    {
        var result = await _service.GetDashboardDataAsync();
        return Ok(result);
    }

    [HttpGet("reports")]
    public async Task<ActionResult<DashboardResponseDto>> GetReports([FromQuery] string period = "month")
    {
        var result = await _service.GetReportsDataAsync(period);
        return Ok(result);
    }

    [HttpGet("reports/export")]
    public async Task<IActionResult> ExportReports([FromQuery] string period = "month", [FromQuery] string format = "csv")
    {
        var data = await _service.GetReportsDataAsync(period);
        
        if (format.ToLower() == "csv")
        {
            var csv = GenerateCsv(data, period);
            var fileName = $"BaoCao_{period}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            return File(System.Text.Encoding.UTF8.GetBytes(csv), "text/csv", fileName);
        }
        else
        {
            // For Excel, you would need a library like EPPlus or ClosedXML
            // For now, return CSV
            var csv = GenerateCsv(data, period);
            var fileName = $"BaoCao_{period}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            return File(System.Text.Encoding.UTF8.GetBytes(csv), "text/csv", fileName);
        }
    }

    private string GenerateCsv(DashboardResponseDto data, string period)
    {
        var csv = new System.Text.StringBuilder();
        
        // Header
        csv.AppendLine("BÁO CÁO THỐNG KÊ");
        csv.AppendLine($"Kỳ báo cáo: {period}");
        csv.AppendLine($"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
        csv.AppendLine();
        
        // Summary
        csv.AppendLine("TỔNG QUAN");
        csv.AppendLine($"Tổng doanh thu,{data.Stats.TotalRevenue:N0}");
        csv.AppendLine($"Thay đổi doanh thu,{data.Stats.RevenueChangePercent}%");
        csv.AppendLine($"Tổng đơn hàng,{data.Stats.TotalOrders}");
        csv.AppendLine($"Thay đổi đơn hàng,{data.Stats.OrdersChangePercent}%");
        csv.AppendLine($"Tổng khách hàng,{data.Stats.TotalCustomers}");
        csv.AppendLine($"Thay đổi khách hàng,{data.Stats.CustomersChangePercent}%");
        csv.AppendLine();
        
        // Top Products
        csv.AppendLine("TOP SẢN PHẨM BÁN CHẠY");
        csv.AppendLine("STT,Tên sản phẩm,Đã bán,Doanh thu");
        for (int i = 0; i < data.TopProducts.Count; i++)
        {
            var p = data.TopProducts[i];
            csv.AppendLine($"{i + 1},{p.Name},{p.Sold},{p.Revenue:N0}");
        }
        csv.AppendLine();
        
        // Monthly Revenue
        csv.AppendLine("DOANH THU THEO THỜI GIAN");
        csv.AppendLine("Thời gian,Doanh thu");
        data.MonthlyRevenue.ForEach(m => {
            csv.AppendLine($"{m.Month},{m.Revenue:N0}");
        });
        csv.AppendLine();
        
        // Category Revenue
        csv.AppendLine("DOANH THU THEO DANH MỤC");
        csv.AppendLine("Danh mục,Doanh thu,Tỷ lệ (%)");
        data.CategoryRevenue.ForEach(c => {
            csv.AppendLine($"{c.CategoryName},{c.Revenue:N0},{c.Percentage}");
        });
        
        return csv.ToString();
    }
}

