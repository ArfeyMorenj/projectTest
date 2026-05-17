using LoginApp.API.Models;

namespace LoginApp.API.Services;

// Ini kontrak service, mirip interface/contract di Laravel jika kita pakai abstraction.
public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);

    Task<AuthResponse> RegisterAsync(RegisterRequest request);
}
