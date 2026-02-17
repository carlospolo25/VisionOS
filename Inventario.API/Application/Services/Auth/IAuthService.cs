using Inventario.API.Application.DTOs.Auth;

namespace Inventario.API.Application.Services.Auth;
public interface IAuthService
{
    AuthResponseDto Login(LoginDto dto);
    void Register (RegisterDto dto);    

    string RefreshToken (String ExpiredJWT);
}
