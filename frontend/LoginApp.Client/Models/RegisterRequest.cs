namespace LoginApp.Client.Models;

/// <summary>
/// DTO untuk request register.
/// 
/// Setara dengan TypeScript interface di Next.js:
/// interface RegisterRequest {
///   username: string;        // min 3 chars
///   password: string;        // min 6 chars
///   email?: string;          // optional
/// }
/// 
/// Penggunaan:
/// - Dikirim ke backend via POST /api/auth/register
/// - Di-serialize ke JSON oleh HttpClient secara otomatis
/// - Validasi dilakukan di frontend dan backend
/// </summary>
public class RegisterRequest
{
    /// <summary>
    /// Username untuk register (wajib diisi, min 3 karakter)
    /// Setara dengan: username: string (required di TypeScript)
    /// Validasi dilakukan di component Register.razor
    /// </summary>
    public required string Username { get; set; }

    /// <summary>
    /// Password untuk register (wajib diisi, min 6 karakter)
    /// Setara dengan: password: string (required di TypeScript)
    /// Validasi dilakukan di component Register.razor
    /// </summary>
    public required string Password { get; set; }

    /// <summary>
    /// Email (opsional)
    /// Setara dengan: email?: string (optional di TypeScript)
    /// Bisa null/empty jika tidak ingin memberikan email
    /// </summary>
    public string? Email { get; set; }
}
