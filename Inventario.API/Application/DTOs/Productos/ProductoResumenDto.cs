namespace Inventario.API.Application.DTOs.Productos;

public class ProductoResumenDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion {  get; set; } = string.Empty;
    public int Stock { get; set; }
    public decimal Precio { get; set; }
    public bool Activo { get; set; }

}

