using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace Data.Repositories;

public class AccountRepo : Repository<Account>, IAccountRepo
{
    public AccountRepo(SneakerShopContext context) : base(context)
    {
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _dbSet.AnyAsync(a => a.Email == email);
    }

    public async Task AddAccountRoleAsync(int accountId, int roleId)
    {
        var accountRole = new AccountRole { AccountId = accountId, RoleId = roleId };
        await _context.AccountRoles.AddAsync(accountRole);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAccountRoleAsync(int accountId, int roleId)
    {
        var accountRole = await _context.AccountRoles
            .FirstOrDefaultAsync(ar => ar.AccountId == accountId && ar.RoleId == roleId);
        if (accountRole != null)
        {
            _context.AccountRoles.Remove(accountRole);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Account?> GetByIdWithRolesAsync(int id)
    {
        return await _dbSet
            .Include(a => a.AccountRoles)
                .ThenInclude(ar => ar.Role)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}
