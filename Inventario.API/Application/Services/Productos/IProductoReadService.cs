using Inventario.API.Application.DTOs.Productos;

namespace Inventario.API.Application.Services.Productos;
public interface IProductoReadService
{
    Task<ProductoResumenDto> ObtenerPorIdAsync (int id);
}

