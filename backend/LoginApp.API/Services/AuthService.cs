using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoginApp.API.Data;
using LoginApp.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LoginApp.API.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        // Setara dengan: User::where('username', $request->username)->first()
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user is null)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Username atau password salah."
            };
        }

        // Setara dengan Hash::check($password, $user->password)
        var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Username atau password salah."
            };
        }

        return new AuthResponse
        {
            Success = true,
            Message = "Login berhasil.",
            Username = user.Username,
            Token = GenerateJwtToken(user)
        };
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        // Setara dengan User::where('username', $request->username)->exists()
        var usernameExists = await _dbContext.Users.AnyAsync(u => u.Username == request.Username);

        if (usernameExists)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Username sudah digunakan."
            };
        }

        // Setara dengan Hash::make($request->password)
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            Username = request.Username,
            PasswordHash = passwordHash,
            Email = request.Email,
            CreatedAt = DateTime.Now
        };

        // Setara dengan $user->save() / User::create()
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return new AuthResponse
        {
            Success = true,
            Message = "Register berhasil.",
            Username = user.Username,
            Token = GenerateJwtToken(user)
        };
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSection = _configuration.GetSection("Jwt");
        var key = jwtSection["Key"] ?? string.Empty;
        var issuer = jwtSection["Issuer"] ?? string.Empty;
        var audience = jwtSection["Audience"] ?? string.Empty;

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
