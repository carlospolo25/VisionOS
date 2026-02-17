using Inventario.API.Application.DTOs.Productos;

namespace Inventario.API.Application.DTOs.Reportes;

public class ReporteInventarioDto
{
    public int TotalProductos { get; set; }
    public int Activos { get; set; }
    public int Inactivos { get; set; }
    public int ConStock { get; set; }
    public int StockCritico { get; set; }
    public int SinStock { get; set; }
    public decimal ValorInventario { get; set; }
    public double PorcentajeRiesgo { get; set; }

    public List<ProductoResumenDto> ProductosEnRiesgo { get; set; } = new();
    public List<ProductoResumenDto> Productos { get; set; } = new();
}

