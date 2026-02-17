using Inventario.API.Application.DTOs.Productos;
using Inventario.API.Domain.Entities;

namespace Inventario.API.Application.Services.Productos;

public interface IProductosService
{
    Task<List<ProductoResumenDto>> ObtenerTodoAsync(string busqueda = "");
}
