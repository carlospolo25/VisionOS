using Inventario.API.Application.DTOs.Productos;
using Microsoft.EntityFrameworkCore;


namespace Inventario.API.Application.Services.Productos;

public class ProductoReadService : IProductoReadService
{
    private readonly IProductoQueryService _query;

    public ProductoReadService(IProductoQueryService query)
    {
        _query = query;
    }

    public async Task<ProductoResumenDto> ObtenerPorIdAsync (int  id)
    {
        return await _query.Query()
            .Where(P => P.Id == id)
            .Select(p => new ProductoResumenDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Precio = p.Precio ?? 0,
                Stock = p.Stock,
                Activo = p.Activo,
            })
            .FirstOrDefaultAsync();

    }

}

