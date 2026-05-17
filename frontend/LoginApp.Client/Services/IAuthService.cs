using LoginApp.Client.Models;

namespace LoginApp.Client.Services;

/// <summary>
/// Interface untuk Authentication Service.
/// 
/// Setara dengan TypeScript dalam Next.js:
/// type AuthService = {
///   login: (req: LoginRequest) => Promise<AuthResponse>;
///   register: (req: RegisterRequest) => Promise<AuthResponse>;
///   setCurrentUser: (username: string, token?: string) => void;
///   getCurrentUser: () => string | null;
///   isLoggedIn: () => boolean;
///   logout: () => void;
/// }
/// 
/// Ini adalah contract yang harus diimplementasi oleh AuthService.
/// Gunakan Dependency Injection untuk inject service ini ke component.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Melakukan login ke backend.
    /// 
    /// Setara dengan: const login = async (req) => await fetch('/api/auth/login', ...)
    /// 
    /// Proses:
    /// 1. POST request ke backend dengan LoginRequest
    /// 2. Backend validate username/password
    /// 3. Backend return AuthResponse (success/failed)
    /// 4. Frontend parse response dan return ke caller
    /// 
    /// Return: AuthResponse (dengan success, message, username, token)
    /// </summary>
    Task<AuthResponse> LoginAsync(LoginRequest request);

    /// <summary>
    /// Melakukan register user baru ke backend.
    /// 
    /// Setara dengan: const register = async (req) => await fetch('/api/auth/register', ...)
    /// 
    /// Proses:
    /// 1. POST request ke backend dengan RegisterRequest
    /// 2. Backend validate username/email/password
    /// 3. Backend create user jika valid
    /// 4. Backend return AuthResponse (success/failed)
    /// 5. Frontend parse response dan return ke caller
    /// 
    /// Return: AuthResponse (dengan success, message)
    /// </summary>
    Task<AuthResponse> RegisterAsync(RegisterRequest request);

    /// <summary>
    /// Menyimpan data user yang login ke dalam memory.
    /// 
    /// Setara dengan: sessionStorage.setItem('user', username)
    ///               atau Redux store, Context API, zustand, dll
    /// 
    /// Perhatian: Ini hanya menyimpan di memory (tidak persistent).
    /// Jika page di-refresh, data akan hilang.
    /// Untuk persistent storage, gunakan ProtectedLocalStorage di Blazor WASM.
    /// 
    /// Parameter:
    /// - username: nama user yang baru login
    /// - token: (future use) JWT token dari backend (opsional)
    /// </summary>
    void SetCurrentUser(string username, string? token = null);

    /// <summary>
    /// Mendapatkan username user yang sedang login.
    /// 
    /// Setara dengan: sessionStorage.getItem('user')
    ///               atau Redux selector, useContext hook, zustand store, dll
    /// 
    /// Return: Username jika ada yang login, null jika tidak ada.
    /// </summary>
    string? GetCurrentUser();

    /// <summary>
    /// Mengecek apakah ada user yang sedang login.
    /// 
    /// Setara dengan: !!sessionStorage.getItem('user')
    ///               atau boolean selector di Redux/Context
    /// 
    /// Return: true jika ada user, false jika tidak ada.
    /// </summary>
    bool IsLoggedIn();

    /// <summary>
    /// Melakukan logout (menghapus session user).
    /// 
    /// Setara dengan: sessionStorage.removeItem('user')
    ///               atau Redux dispatch(logout()), Context setState(null)
    /// 
    /// Proses:
    /// 1. Hapus data user dari memory
    /// 2. Component akan navigate ke /login
    /// </summary>
    void Logout();
}
