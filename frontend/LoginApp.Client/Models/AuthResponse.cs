namespace LoginApp.Client.Models;

/// <summary>
/// DTO untuk response dari backend (login/register/logout).
/// 
/// Setara dengan TypeScript interface di Next.js:
/// interface AuthResponse {
///   success: boolean;
///   message: string;
///   username?: string;
///   token?: string;
/// }
/// 
/// Penggunaan:
/// - Diterima dari backend response JSON
/// - Di-deserialize otomatis oleh HttpClient ke object C#
/// - Setara dengan: const data = await response.json() di TypeScript
/// 
/// Contoh Response dari Backend:
/// {
///   "success": true,
///   "message": "Login berhasil",
///   "username": "john_doe",
///   "token": "eyJhbGc..."
/// }
/// 
/// atau (jika gagal):
/// {
///   "success": false,
///   "message": "Username atau password salah",
///   "username": null,
///   "token": null
/// }
/// </summary>
public class AuthResponse
{
    /// <summary>
    /// Status keberhasilan request (true/false)
    /// Setara dengan: success: boolean di TypeScript
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Pesan dari backend (error message atau success message)
    /// Setara dengan: message: string di TypeScript
    /// Contoh: "Login berhasil", "Password salah", "User sudah terdaftar"
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Username yang login (jika sukses)
    /// Setara dengan: username?: string di TypeScript (optional)
    /// Bisa null jika login gagal atau saat logout
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// JWT Token dari backend (jika sukses login)
    /// Setara dengan: token?: string di TypeScript (optional)
    /// Untuk future use: bisa disimpan di localStorage/sessionStorage
    /// atau di ProtectedLocalStorage di Blazor WASM
    /// </summary>
    public string? Token { get; set; }
}
