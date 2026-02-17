using Inventario.API.Domain.Entities;
using Inventario.API.Infrastructure.Data;
using Inventario.API.Application.DTOs.Productos;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Inventario.API.Application.Services.Productos;

public class ProductosService : IProductosService
{
    private readonly AppDbContext _context;

    public ProductosService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductoResumenDto>> ObtenerTodoAsync(string busqueda = "")
    {
        var query = _context.Producto.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            query = query.Where(p =>
                p.Nombre.Contains(busqueda));
        }

        return await query
            .Select(p => new ProductoResumenDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Precio = p.Precio ?? 0m,
                Stock = p.Stock,
                Activo = p.Activo
            })
            .ToListAsync();
    }



}

