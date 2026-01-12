using Business.DTOs;

namespace Business.Interfaces;

public interface IAccountService
{
    Task<IEnumerable<AccountResponseDto>> GetAllAsync();
    Task<AccountResponseDto?> GetByIdAsync(int id);
    Task<AccountResponseDto> CreateAsync(AccountCreateDto dto);
    Task<bool> UpdateAsync(int id, AccountUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ChangePasswordAsync(int id, AccountChangePasswordDto dto);
}

