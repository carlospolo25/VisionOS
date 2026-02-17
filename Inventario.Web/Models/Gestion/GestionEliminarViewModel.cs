using Inventario.Web.Models.Gestion;
using System.Collections.Generic;

namespace Inventario.Web.Models
{
    public class GestionEliminarViewModel
    {
        public List<GestionViewModel> Detalles { get; set; } = new();
    }
}
