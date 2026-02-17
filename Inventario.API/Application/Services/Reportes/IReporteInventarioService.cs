using Inventario.API.Application.DTOs.Reportes;

namespace Inventario.API.Application.Services.Reportes;

public interface IReporteInventarioService
{
    Task<ReporteInventarioDto> ObtenerReporteAsync();
}

