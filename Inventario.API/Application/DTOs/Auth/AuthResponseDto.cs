namespace Inventario.API.Application.DTOs.Auth;

public class AuthResponseDto
{
    public string Token { get; set; } = null!;
    public string Rol { get; set; } = null!;
}
