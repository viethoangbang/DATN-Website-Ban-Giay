using Data.Models;

namespace Data.Interfaces;

public interface IAccountRepo : IRepository<Account>
{
    Task<bool> EmailExistsAsync(string email);
    Task AddAccountRoleAsync(int accountId, int roleId);
    Task RemoveAccountRoleAsync(int accountId, int roleId);
}
