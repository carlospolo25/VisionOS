namespace Inventario.API.Domain.Entities;

public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; } 
    public int Stock { get; set; }
    public decimal? Precio { get; set; }
    public bool Activo { get; set; } =true;
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
}

