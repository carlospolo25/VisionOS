
namespace Inventario.API.Infrastructure;
public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public bool Revoked { get; set; }

    public int UserId { get; set; }
}

