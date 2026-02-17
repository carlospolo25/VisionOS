using Inventario.Web.Models.Productos;

namespace Inventario.Web.Models.Reportes;

public class ReportesViewModel
{
    public int TotalProductos { get; set; }
    public int Activos { get; set; }
    public int Inactivos { get; set; }
    public int ConStock { get; set; }
    public int StockCritico { get; set; }
    public int SinStock { get; set; }
    public decimal ValorInventario { get; set; }
    public double PorcentajeRiesgo { get; set; }

    public List<ProductosViewModel> ProductosEnRiesgo { get; set; } = new();
    public List<ProductosViewModel> Productos { get; set; } = new();

}

