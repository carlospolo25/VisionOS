namespace Inventario.Web.Models.Gestion;

public class EditarGestionViewModel
{
    public List<GestionViewModel> Productos { get; set; } = new();
    public GestionViewModel ProductoEditar { get; set; }
    public string Busqueda { get; set; }
}

