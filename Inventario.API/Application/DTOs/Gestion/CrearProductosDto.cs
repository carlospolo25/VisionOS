namespace Inventario.API.Application.DTOs.Gestion
{
    public class CrearProductosDto
    {
        public string Nombre { get; set; }
        public string Descripcion {  get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
    }
}
