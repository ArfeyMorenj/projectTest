using System.Text;
using System.Text.Json;
using LoginApp.Client.Models;

namespace LoginApp.Client.Services;

/// <summary>
/// Implementasi Authentication Service.
/// 
/// Setara dengan lib/auth.ts di Next.js yang berisi:
/// - export const login = async (req) => { ... }
/// - export const register = async (req) => { ... }
/// - export const useAuth = () => { ... }
/// 
/// Service ini menghandle:
/// 1. HTTP Communication dengan backend (POST ke /api/auth/*)
/// 2. JSON serialization/deserialization (otomatis via HttpClient)
/// 3. Session management (simpan/ambil user state)
/// 4. Error handling
/// 
/// Cara Penggunaan:
/// - Inject di Program.cs: builder.Services.AddScoped<IAuthService, AuthService>();
/// - Pakai di component: @inject IAuthService authService
/// - Call method: var response = await authService.LoginAsync(request);
/// </summary>
public class AuthService : IAuthService
{
    // ─────────────────────────────────────────────────────────────
    // PRIVATE FIELDS
    // ─────────────────────────────────────────────────────────────

    /// <summary>
    /// HttpClient untuk komunikasi dengan backend.
    /// Di-inject dari Program.cs sudah dikonfigurasi dengan BaseAddress.
    /// 
    /// Setara dengan: const api = axios.create({ baseURL: '...' })
    /// </summary>
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Static field untuk menyimpan username user yang login (in-memory).
    /// 
    /// Setara dengan: let currentUser = null; di module scope
    ///               atau Redux store, Context API state
    /// 
    /// Catatan: Data ini TIDAK persistent (akan hilang saat page refresh).
    /// Untuk production, gunakan ProtectedLocalStorage di Blazor WASM.
    /// 
    /// Static agar bisa di-access across component instances
    /// dan tetap tersimpan selama app berjalan.
    /// </summary>
    private static string? _currentUser = null;

    // ─────────────────────────────────────────────────────────────
    // CONSTRUCTOR
    // ─────────────────────────────────────────────────────────────

    /// <summary>
    /// Constructor yang menerima HttpClient via Dependency Injection.
    /// 
    /// Setara dengan:
    /// const api = axios.create({ baseURL: '...' });
    /// export function AuthService(httpClient) { ... }
    /// 
    /// HttpClient sudah dikonfigurasi di Program.cs dengan BaseAddress
    /// ke backend (https://localhost:7001)
    /// </summary>
    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // ─────────────────────────────────────────────────────────────
    // PUBLIC METHODS
    // ─────────────────────────────────────────────────────────────

    /// <summary>
    /// Implementasi LoginAsync dari IAuthService.
    /// 
    /// Setara dengan:
    /// export const login = async (request: LoginRequest) => {
    ///   const res = await fetch('/api/auth/login', {
    ///     method: 'POST',
    ///     headers: { 'Content-Type': 'application/json' },
    ///     body: JSON.stringify(request)
    ///   });
    ///   return await res.json();
    /// }
    /// </summary>
    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            // 1. Serialize request ke JSON
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // 2. POST ke backend: POST /api/auth/login
            // HttpClient sudah punya BaseAddress dari Program.cs
            // Jadi cukup gunakan relative URL: "api/auth/login"
            var response = await _httpClient.PostAsync("api/auth/login", content);

            // 3. Baca response body sebagai string
            var responseBody = await response.Content.ReadAsStringAsync();

            // 4. Deserialize JSON string ke AuthResponse object
            var authResponse = JsonSerializer.Deserialize<AuthResponse>(
                responseBody,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            // Jika response null (error parsing), return error response
            if (authResponse == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Tidak dapat parse response dari server"
                };
            }

            // 5. Jika login sukses, simpan username ke memory
            if (authResponse.Success && !string.IsNullOrEmpty(authResponse.Username))
            {
                SetCurrentUser(authResponse.Username, authResponse.Token);
            }

            return authResponse;
        }
        catch (HttpRequestException ex)
        {
            // Error saat berkomunikasi dengan server (network error, server down, dll)
            return new AuthResponse
            {
                Success = false,
                Message = $"Error komunikasi dengan server: {ex.Message}"
            };
        }
        catch (JsonException ex)
        {
            // Error saat parse JSON
            return new AuthResponse
            {
                Success = false,
                Message = $"Error parse response: {ex.Message}"
            };
        }
        catch (Exception ex)
        {
            // Error lain-lain
            return new AuthResponse
            {
                Success = false,
                Message = $"Terjadi error: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Implementasi RegisterAsync dari IAuthService.
    /// 
    /// Setara dengan:
    /// export const register = async (request: RegisterRequest) => {
    ///   const res = await fetch('/api/auth/register', {
    ///     method: 'POST',
    ///     headers: { 'Content-Type': 'application/json' },
    ///     body: JSON.stringify(request)
    ///   });
    ///   return await res.json();
    /// }
    /// </summary>
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        try
        {
            // 1. Serialize request ke JSON
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // 2. POST ke backend: POST /api/auth/register
            var response = await _httpClient.PostAsync("api/auth/register", content);

            // 3. Baca response body sebagai string
            var responseBody = await response.Content.ReadAsStringAsync();

            // 4. Deserialize JSON string ke AuthResponse object
            var authResponse = JsonSerializer.Deserialize<AuthResponse>(
                responseBody,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            // Jika response null, return error response
            if (authResponse == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Tidak dapat parse response dari server"
                };
            }

            // Catatan: Setelah register sukses, user harus login manual
            // (tidak auto-login karena security reasons)
            // Jadi jangan panggil SetCurrentUser di sini

            return authResponse;
        }
        catch (HttpRequestException ex)
        {
            return new AuthResponse
            {
                Success = false,
                Message = $"Error komunikasi dengan server: {ex.Message}"
            };
        }
        catch (JsonException ex)
        {
            return new AuthResponse
            {
                Success = false,
                Message = $"Error parse response: {ex.Message}"
            };
        }
        catch (Exception ex)
        {
            return new AuthResponse
            {
                Success = false,
                Message = $"Terjadi error: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Implementasi SetCurrentUser dari IAuthService.
    /// 
    /// Setara dengan:
    /// sessionStorage.setItem('user', username);
    /// atau: setUser(username) di Redux/Context
    /// </summary>
    public void SetCurrentUser(string username, string? token = null)
    {
        // Simpan username ke static field (in-memory)
        // Bisa di-access dari semua instance AuthService
        _currentUser = username;

        // Note: Token bisa disimpan ke ProtectedLocalStorage jika diperlukan
        // untuk persistent authentication across page refresh
        // Contoh:
        // await protectedLocalStorage.SetAsync("token", token);
    }

    /// <summary>
    /// Implementasi GetCurrentUser dari IAuthService.
    /// 
    /// Setara dengan:
    /// sessionStorage.getItem('user');
    /// atau: useSelector(state => state.auth.user) di Redux
    /// </summary>
    public string? GetCurrentUser()
    {
        // Return username yang tersimpan di static field
        return _currentUser;
    }

    /// <summary>
    /// Implementasi IsLoggedIn dari IAuthService.
    /// 
    /// Setara dengan:
    /// !!sessionStorage.getItem('user')
    /// atau: useSelector(state => !!state.auth.user)
    /// </summary>
    public bool IsLoggedIn()
    {
        // Return true jika ada user yang login
        return !string.IsNullOrEmpty(_currentUser);
    }

    /// <summary>
    /// Implementasi Logout dari IAuthService.
    /// 
    /// Setara dengan:
    /// sessionStorage.removeItem('user');
    /// atau: dispatch(logoutUser()) di Redux
    /// </summary>
    public void Logout()
    {
        // Hapus data user dari memory
        _currentUser = null;

        // Note: Bisa juga hapus token dari ProtectedLocalStorage jika ada:
        // await protectedLocalStorage.DeleteAsync("token");
    }
}
