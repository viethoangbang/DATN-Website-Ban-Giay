using Business.DTOs;

namespace Business.Interfaces;

public interface IDashboardService
{
    Task<DashboardResponseDto> GetDashboardDataAsync();
    Task<DashboardResponseDto> GetReportsDataAsync(string period);
}

