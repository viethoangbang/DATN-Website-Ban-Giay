using Business.DTOs;

namespace Business.Interfaces;

public interface IColorService
{
    Task<IEnumerable<ColorResponseDto>> GetAllAsync();
    Task<ColorResponseDto?> GetByIdAsync(int id);
    Task<ColorResponseDto> CreateAsync(ColorCreateDto dto);
    Task<bool> UpdateAsync(int id, ColorUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
