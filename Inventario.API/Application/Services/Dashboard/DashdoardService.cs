using Inventario.API.Application.DTOs.Dashboard;
using Inventario.API.Application.DTOs.Productos;
using Inventario.API.Application.Services.Productos;
using Inventario.API.Domain.Entities;
using Inventario.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Inventario.API.Application.Services.Dashboard;

public class DashdoardService : IDashboardService
{
    private readonly IProductoQueryService _productoQuery;

    public DashdoardService(IProductoQueryService productoQuery)
    {
        _productoQuery = productoQuery;
    }
    public async Task<DashboardDto> Productos()
    {
        var query = _productoQuery.Query();

        var totalProductos = await _productoQuery
            .Query()
            .CountAsync();

        var productosConStock = await query
            .CountAsync(p => p.Stock >= 1);

        var stockCritico = await query
            .CountAsync(p => p.Stock > 0 && p.Stock <= 5);

        var sinStock = await query
            .CountAsync(p => p.Stock == 0);
        
        var valorInventario = await query
            .Where(p => p.Activo && p.Stock >= 1)
            .SumAsync(p => p.Stock * (p.Precio ?? 0m));

        var productosEnRiesgo = await query
            .Where(p => p.Stock <= 5)
            .OrderBy(p => p.Stock)
            .Select(p => new ProductoResumenDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Stock = p.Stock,
                Precio = p.Precio ?? 0m,
                Activo = p.Activo

            })
            .Take(5)
            .ToListAsync();

        var totalEnRiesgo = stockCritico + sinStock;

        double porcentajeRiesgo = 0;

        if (totalEnRiesgo > 0)
        {
            porcentajeRiesgo = (double)totalEnRiesgo / totalProductos * 100;
        }


        return new DashboardDto
        {
            TotalProductos = totalProductos,
            ProductosConStock = productosConStock,
            StockCritico = stockCritico,
            SinStock = sinStock,
            ValorInventario = valorInventario,
            ProductosEnRiesgo = productosEnRiesgo,
            PorcentajeRiesgo = porcentajeRiesgo,
        };

    }
}

