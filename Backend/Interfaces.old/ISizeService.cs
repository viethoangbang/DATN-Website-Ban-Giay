using Business.DTOs;

namespace Business.Interfaces;

public interface ISizeService
{
    Task<IEnumerable<SizeResponseDto>> GetAllAsync();
    Task<SizeResponseDto?> GetByIdAsync(int id);
    Task<SizeResponseDto> CreateAsync(SizeCreateDto dto);
    Task<bool> UpdateAsync(int id, SizeUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
