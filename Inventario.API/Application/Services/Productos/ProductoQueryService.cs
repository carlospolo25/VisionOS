
using Inventario.API.Domain.Entities;
using Inventario.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventario.API.Application.Services.Productos;

public class ProductoQueryService : IProductoQueryService
{
    public readonly AppDbContext _context;

    public ProductoQueryService (AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<Producto> Query ()
    {
        return _context.Producto.AsNoTracking();
    }
}

