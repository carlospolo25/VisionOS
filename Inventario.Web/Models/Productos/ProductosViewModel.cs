namespace Inventario.Web.Models.Productos
{
    public class ProductosViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion {  get; set; }
        public int Stock { get; set; }
        public decimal Precio { get; set; }
        public bool Activo { get; set; }
    }

}



