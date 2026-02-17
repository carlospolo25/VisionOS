using Inventario.Web.Models.Productos;

namespace Inventario.Web.Models.Dashboard;
public class DashboardViewModel
{
    public int TotalProductos { get; set; }
    public int ProductosConStock { get; set; }
    public int StockCritico { get; set; }
    public int SinStock { get; set; }
    public decimal ValorInventario { get; set; }
    public double PorcentajeRiesgo { get; set; }
    public List<ProductosViewModel> ProductosEnRiesgo { get; set; } = new();

    public List<ProductosViewModel> Productos { get; set; } = new();
}

