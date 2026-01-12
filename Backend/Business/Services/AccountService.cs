using Business.DTOs;
using Business.Interfaces;
using Business.Exceptions;
using Data.Interfaces;
using Data.Models;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepo _accountRepo;
    private readonly SneakerShopContext _context;

    public AccountService(IAccountRepo accountRepo, SneakerShopContext context)
    {
        _accountRepo = accountRepo;
        _context = context;
    }

    public async Task<IEnumerable<AccountResponseDto>> GetAllAsync()
    {
        try
        {
            var accounts = await _accountRepo.GetAllAsync();
            return accounts.Select(a => MapToDto(a)).ToList();
        }
        catch (Exception ex)
        {
            // Log error
            throw new Exception($"Error getting accounts: {ex.Message}", ex);
        }
    }

    public async Task<AccountResponseDto?> GetByIdAsync(int id)
    {
        var account = await _accountRepo.GetByIdAsync(id);
        return account == null ? null : MapToDto(account);
    }

    public async Task<AccountResponseDto> CreateAsync(AccountCreateDto dto)
    {
        // Check if email exists
        if (await _accountRepo.EmailExistsAsync(dto.Email))
            throw new BadRequestException("Email already exists");

        var status = dto.Status ?? "Active";
        var account = new Account
        {
            Email = dto.Email,
            Password = HashPassword(dto.Password),
            PhoneNumber = dto.PhoneNumber,
            FullName = dto.FullName,
            Sex = dto.Sex,
            BirthYear = dto.BirthYear,
            AvatarUrl = dto.AvatarUrl,
            Status = status,
            CreateDate = DateTime.Now
        };

        var created = await _accountRepo.AddAsync(account);

        // Add roles
        if (dto.Roles != null && dto.Roles.Count > 0)
        {
            foreach (var roleName in dto.Roles)
            {
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name!.ToLower() == roleName.ToLower());
                if (role != null)
                {
                    await _accountRepo.AddAccountRoleAsync(created.Id, role.Id);
                }
            }
        }

        return MapToDto(await _accountRepo.GetByIdAsync(created.Id)!);
    }

    public async Task<bool> UpdateAsync(int id, AccountUpdateDto dto)
    {
        var account = await _accountRepo.GetByIdAsync(id);
        if (account == null)
            return false;

        // Update basic info
        if (!string.IsNullOrEmpty(dto.PhoneNumber))
            account.PhoneNumber = dto.PhoneNumber;
        if (!string.IsNullOrEmpty(dto.FullName))
            account.FullName = dto.FullName;
        if (!string.IsNullOrEmpty(dto.Sex))
            account.Sex = dto.Sex;
        if (dto.BirthYear.HasValue)
            account.BirthYear = dto.BirthYear;
        if (dto.AvatarUrl != null)
            account.AvatarUrl = dto.AvatarUrl;
        
        // Update Status
        if (!string.IsNullOrEmpty(dto.Status))
            account.Status = dto.Status;

        account.UpdateDate = DateTime.Now;

        // Update roles if provided
        if (dto.Roles != null && dto.Roles.Count > 0)
        {
            // Remove all existing roles
            var existingRoles = account.AccountRoles.ToList();
            foreach (var accountRole in existingRoles)
            {
                if (accountRole.RoleId.HasValue)
                {
                    await _accountRepo.RemoveAccountRoleAsync(account.Id, accountRole.RoleId.Value);
                }
            }

            // Add new roles
            foreach (var roleName in dto.Roles)
            {
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name!.ToLower() == roleName.ToLower());
                if (role != null)
                {
                    await _accountRepo.AddAccountRoleAsync(account.Id, role.Id);
                }
            }
        }

        await _accountRepo.UpdateAsync(account);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var account = await _accountRepo.GetByIdAsync(id);
        if (account == null)
            return false;

        await _accountRepo.DeleteAsync(account);
        return true;
    }

    public async Task<bool> ChangePasswordAsync(int id, AccountChangePasswordDto dto)
    {
        var account = await _accountRepo.GetByIdAsync(id);
        if (account == null)
            return false;

        // Verify current password
        if (!VerifyPassword(dto.CurrentPassword, account.Password))
            throw new BadRequestException("Current password is incorrect");

        // Update password
        account.Password = HashPassword(dto.NewPassword);
        account.UpdateDate = DateTime.Now;
        await _accountRepo.UpdateAsync(account);

        return true;
    }

    private AccountResponseDto MapToDto(Account account)
    {
        if (account == null)
            throw new ArgumentNullException(nameof(account));

        var roles = new List<string>();
        if (account.AccountRoles != null)
        {
            roles = account.AccountRoles
                .Where(ar => ar != null && ar.Role != null && !string.IsNullOrEmpty(ar.Role.Name))
                .Select(ar => ar.Role!.Name!)
                .ToList();
        }

        return new AccountResponseDto
        {
            Id = account.Id,
            Email = account.Email,
            PhoneNumber = account.PhoneNumber,
            FullName = account.FullName,
            Sex = account.Sex,
            BirthYear = account.BirthYear,
            AvatarUrl = account.AvatarUrl,
            Status = account.Status,
            Roles = roles,
            CreateDate = account.CreateDate,
            UpdateDate = account.UpdateDate
        };
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPassword(string password, string? hashedPassword)
    {
        if (string.IsNullOrEmpty(hashedPassword))
            return false;
        
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}

