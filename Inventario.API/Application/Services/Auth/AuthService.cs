using Inventario.API.Application.DTOs.Auth;
using Inventario.API.Domain.Entities;
using Inventario.API.Infrastructure;
using Inventario.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Inventario.API.Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public AuthResponseDto Login(LoginDto dto)
    {
        var usuario = _context.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefault(u => u.Email == dto.Email);

        if (usuario == null)
        {
            throw new Exception("Usuario no encontrado");
        }

        if (usuario.PasswordHash.Trim() != dto.Password.Trim())
        {
            throw new Exception("Contraseña incorrecta");
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Role, usuario.Rol.Nombre)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
        );

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        var refreshToken = new RefreshToken
        {
            Token = Guid.NewGuid().ToString(),
            UserId = usuario.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            Revoked = false

        };

        _context.RefreshTokens.Add(refreshToken);
        _context.SaveChanges();

        return new AuthResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Rol = usuario.Rol.Nombre
        };
    }

    public void Register(RegisterDto dto)
    {
        if (_context.Usuarios.Any(u => u.Email == dto.Email))
        {
            throw new Exception("El Usuario ya existe.");
        }

        var user = new Usuario
        {
            Nombre = dto.Nombre,
            Email = dto.Email.Trim(),
            PasswordHash = dto.Password.Trim(),
            RolId = 2
        };
        _context.Usuarios.Add(user);
        _context.SaveChanges();
    }

    public string   RefreshToken (string ExpiredJwt)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(ExpiredJwt);

        var UserIdClaim = jwtToken.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        if (UserIdClaim == null)
            throw new SecurityTokenException("Token inválido");

        var userId = int.Parse(UserIdClaim.Value);

        var refreshToken = _context.RefreshTokens.FirstOrDefault(r =>

            r.UserId == userId &&
            !r.Revoked &&
            r.ExpiresAt > DateTime.UtcNow);

        if (refreshToken == null)
            throw new SecurityTokenException("RefreshToken inválido");

        var usuarios = _context.Usuarios
            .Include(u => u.Rol)
            .First(u => u.Id == userId);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuarios.Id.ToString()),
            new Claim (ClaimTypes.Role, usuarios.Rol.Nombre)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
            );

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(5),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);

    }


}
