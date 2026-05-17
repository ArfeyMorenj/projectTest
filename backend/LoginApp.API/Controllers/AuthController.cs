using LoginApp.API.Models;
using LoginApp.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoginApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
    {
        // Setara dengan $request->validate() di Laravel
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var result = await _authService.RegisterAsync(request);

        if (!result.Success)
        {
            return BadRequest(result); // setara response()->json(..., 400)
        }

        return Ok(result); // setara response()->json(...)
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
    {
        // Setara dengan $request->validate() di Laravel
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var result = await _authService.LoginAsync(request);

        if (!result.Success)
        {
            return Unauthorized(result); // setara response()->json(..., 401)
        }

        return Ok(result);
    }
}
