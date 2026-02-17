using Inventario.API.Application.DTOs.Reportes;

namespace Inventario.API.Application.Services.Reportes;

public interface IPdfReporteService
{          
    byte[] GenerarPdf(ReporteInventarioDto reporte);
}

