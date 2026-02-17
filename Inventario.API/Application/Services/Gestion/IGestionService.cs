using Inventario.API.Application.DTOs.Gestion;
using Inventario.API.Domain.Entities;

namespace Inventario.API.Application.Services.Gestion;

public interface IGestionService
{
    Task<int> CrearProductosAsync(CrearProductosDto dto);
    Task<bool> EditarProductosAsync(int id, EditarProductosDto dto);
    Task<bool>EliminarProductosAsync(int id);

    Task<bool> ActivarProductosAsync(int id);
    Task<bool> DesactivarProductosAsync(int id);

}

