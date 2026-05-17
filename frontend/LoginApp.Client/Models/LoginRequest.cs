namespace LoginApp.Client.Models;

/// <summary>
/// DTO untuk request login.
/// 
/// Setara dengan TypeScript interface di Next.js:
/// interface LoginRequest {
///   username: string;
///   password: string;
/// }
/// 
/// Penggunaan:
/// - Dikirim ke backend via POST /api/auth/login
/// - Di-serialize ke JSON oleh HttpClient secara otomatis
/// - Setara dengan: fetch('/api/auth/login', { body: JSON.stringify(request) })
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Username untuk login (wajib diisi)
    /// Setara dengan: username: string (required di TypeScript)
    /// </summary>
    public required string Username { get; set; }

    /// <summary>
    /// Password untuk login (wajib diisi)
    /// Setara dengan: password: string (required di TypeScript)
    /// </summary>
    public required string Password { get; set; }
}
