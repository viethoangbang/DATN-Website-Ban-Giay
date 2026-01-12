using Business.DTOs;
using Business.Interfaces;
using Data.Models;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Business.Services;

public class AuthService : IAuthService
{
    private readonly SneakerShopContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(SneakerShopContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            // Find user by email
            var account = await _context.Accounts
                .Include(a => a.AccountRoles)
                    .ThenInclude(ar => ar.Role)
                .FirstOrDefaultAsync(a => a.Email == request.Email);

            if (account == null)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Invalid email or password"
                };
            }

            // Check account status
            if (account.Status != "Active")
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Account is inactive"
                };
            }

            // Verify password
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, account.Password);
            if (!isValidPassword)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Invalid email or password"
                };
            }

            // Get user roles
            var roles = account.AccountRoles
                .Select(ar => ar.Role.Name)
                .ToList();

            // Generate JWT token
            var token = GenerateJwtToken(account, roles);

            return new LoginResponse
            {
                Success = true,
                Message = "Login successful",
                Token = token,
                User = new UserInfo
                {
                    Id = account.Id,
                    Email = account.Email,
                    FullName = account.FullName,
                    PhoneNumber = account.PhoneNumber,
                    Roles = roles
                }
            };
        }
        catch (Exception ex)
        {
            return new LoginResponse
            {
                Success = false,
                Message = $"Login failed: {ex.Message}"
            };
        }
    }

    private string GenerateJwtToken(Account account, List<string> roles)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new Exception("JWT SecretKey not configured");
        var issuer = jwtSettings["Issuer"] ?? "SneakerShopAPI";
        var audience = jwtSettings["Audience"] ?? "SneakerShopClient";
        var expirationHours = int.Parse(jwtSettings["ExpirationHours"] ?? "24");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
            new Claim(ClaimTypes.Email, account.Email),
            new Claim(ClaimTypes.Name, account.FullName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Add roles to claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(expirationHours),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        try
        {
            // Check if email already exists
            var existingAccount = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Email == request.Email);

            if (existingAccount != null)
            {
                return new RegisterResponse
                {
                    Success = false,
                    Message = "Email đã được sử dụng"
                };
            }

            // Hash password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password, 11);

            // Create new account
            var newAccount = new Account
            {
                Email = request.Email,
                Password = hashedPassword,
                FullName = request.Email.Split('@')[0], // Use email prefix as default name
                Status = "Active",
                CreateDate = DateTime.Now
            };

            _context.Accounts.Add(newAccount);
            await _context.SaveChangesAsync();

            // Assign Customer role by default
            var customerRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Customer");
            if (customerRole != null)
            {
                var accountRole = new AccountRole
                {
                    AccountId = newAccount.Id,
                    RoleId = customerRole.Id
                };
                _context.AccountRoles.Add(accountRole);
                await _context.SaveChangesAsync();
            }

            // Auto login after registration
            var loginResult = await LoginAsync(new LoginRequest
            {
                Email = request.Email,
                Password = request.Password
            });

            return new RegisterResponse
            {
                Success = loginResult.Success,
                Message = loginResult.Success ? "Đăng ký thành công" : loginResult.Message,
                Token = loginResult.Token,
                User = loginResult.User
            };
        }
        catch (Exception ex)
        {
            return new RegisterResponse
            {
                Success = false,
                Message = $"Đăng ký thất bại: {ex.Message}"
            };
        }
    }
}
