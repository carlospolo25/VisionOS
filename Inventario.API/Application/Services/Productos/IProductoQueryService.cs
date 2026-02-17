using Inventario.API.Domain.Entities;

namespace Inventario.API.Application.Services.Productos;


public interface IProductoQueryService
{
    IQueryable<Producto> Query();
}

