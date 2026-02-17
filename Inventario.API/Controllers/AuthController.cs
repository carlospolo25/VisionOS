using Inventario.API.Application.DTOs.Auth;
using Inventario.API.Application.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Inventario.API.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        try
        {
            var result = _authService.Login(dto);
            return Ok(result);
        }
        catch (Exception ex) 
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterDto dto)
    {
        _authService.Register(dto);
        return Ok("Usuario creado");
    }

    [HttpPost("refresh")]
    public IActionResult Refresh()
    {

        var authheader = Request.Headers["Authorization"].ToString();

        if (string.IsNullOrEmpty(authheader) || !authheader.StartsWith("Bearer"))
            return Unauthorized();

        var expiredJwt = authheader.Replace("Bearer", "");

        try
        {
            var newToken = _authService.RefreshToken(expiredJwt);
            return Ok(new { token = newToken });
        }
        catch
        {
            return Unauthorized();
        }
    }
}
