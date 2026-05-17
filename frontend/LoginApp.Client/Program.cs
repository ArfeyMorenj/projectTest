using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using LoginApp.Client;
using LoginApp.Client.Services;

// ═══════════════════════════════════════════════════════════════════════
// KONFIGURASI BLAZOR WEBASSEMBLY FRONTEND
// ═══════════════════════════════════════════════════════════════════════
// 
// Ini setara dengan:
// - next.config.js (konfigurasi Next.js)
// - _app.tsx (setup providers, context, global setup)
// - nuxt.config.ts (konfigurasi Nuxt.js)
// 
// Di sini kita setup semua layanan (Services) yang akan di-inject ke komponen.
// 

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// ─────────────────────────────────────────────────────────────────────
// 1. ROOT COMPONENTS - Mana element HTML yang akan di-render
// ─────────────────────────────────────────────────────────────────────
// Setara dengan: ReactDOM.render(<App />, document.getElementById('app'))
// Di sini kita mount App.razor ke element dengan id="app" di index.html

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ─────────────────────────────────────────────────────────────────────
// 2. DEPENDENCY INJECTION (Services Registration)
// ─────────────────────────────────────────────────────────────────────
// Setara dengan React Context Provider atau Next.js getServerSideProps
// 
// Di sini kita daftarkan semua service yang bisa di-inject ke komponen:
// @inject IAuthService authService
// 

// === 2A. HttpClient dengan BaseAddress ke Backend ===
// Setara dengan: const api = axios.create({ baseURL: 'http://localhost:5162' })
// atau: const fetchOptions = { headers: { 'Authorization': '...' } }
//
// PENTING: BaseAddress di-set ke backend API sesuai Postman collection
// Dari Postman: http://localhost:5162 (HTTP, bukan HTTPS!)
// 
// Development:
//   - Frontend: https://localhost:7002 (Blazor WASM)
//   - Backend:  http://localhost:5162 (ASP.NET Core)
// 
// HttpClient akan otomatis combine: http://localhost:5162 + api/auth/login
builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri("http://localhost:5162/")
});

// === 2B. Daftarkan AuthService ===
// Setara dengan:
// - React Context: <AuthProvider><App /></AuthProvider>
// - Redux: <Provider store={store}><App /></Provider>
// - Next.js: export default withAuth(App)
// 
// AddScoped = buat instance baru untuk setiap HTTP request
// (atau component scope di Blazor WASM)
// 
// AuthService akan di-inject ke dalam Login.razor, Register.razor, dll:
// @inject IAuthService authService
builder.Services.AddScoped<IAuthService, AuthService>();

// ─────────────────────────────────────────────────────────────────────
// 3. BUILD DAN RUN
// ─────────────────────────────────────────────────────────────────────

await builder.Build().RunAsync();
