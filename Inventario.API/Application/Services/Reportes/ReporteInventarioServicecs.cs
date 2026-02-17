using Inventario.API.Infrastructure.Data;
using Inventario.API.Application.DTOs.Reportes;
using Inventario.API.Application.DTOs.Productos;
using Microsoft.EntityFrameworkCore;
namespace Inventario.API.Application.Services.Reportes;

public class ReporteInventarioServicecs : IReporteInventarioService
{
    public readonly AppDbContext _context;

    public ReporteInventarioServicecs (AppDbContext context)
    {
        _context = context;
    }

    public async Task<ReporteInventarioDto> ObtenerReporteAsync()
    {
        var query = _context.Producto.AsNoTracking();

        var total = await query.CountAsync();

        var activos = await query.CountAsync(p => p.Activo);
        var inactivos = await query.CountAsync(p => !p.Activo);

        var conStock = await query.CountAsync(p => p.Stock > 5);
        var critico = await query.CountAsync(p => p.Stock > 0 && p.Stock <= 5);
        var sinStock = await query.CountAsync(p => p.Stock == 0);

        var valor = await query
            .Where(p => p.Activo && p.Stock > 0)
            .SumAsync(p => p.Stock * (p.Precio ?? 0));

        var riesgo = critico + sinStock;

        var porcentaje = total == 0 ? 0 :
            (double)riesgo / total * 100;

        var productosRiesgo = await query
            .Where(p => p.Stock <= 5)
            .OrderBy(p => p.Stock)
            .Select(p => new ProductoResumenDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Stock = p.Stock,
                Precio = p.Precio ?? 0,
                Activo = p.Activo


            })
            .ToListAsync();

        var todos = await query
            .Select (p => new ProductoResumenDto
            {

                Id = p.Id,
                Nombre = p.Nombre,
                Stock = p.Stock,
                Precio = p.Precio ?? 0,
                Activo = p.Activo
            })
            .ToListAsync();

        return new ReporteInventarioDto
        {
            TotalProductos = total,
            Activos = activos,
            Inactivos = inactivos,
            ConStock = conStock,
            StockCritico = critico,
            SinStock = sinStock,
            ValorInventario = valor,
            PorcentajeRiesgo = porcentaje,
            ProductosEnRiesgo = productosRiesgo,
            Productos = todos
        };
    }
}

