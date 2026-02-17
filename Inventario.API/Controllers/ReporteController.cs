using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Inventario.API.Application.Services.Reportes;
using Microsoft.AspNetCore.Authorization;

namespace Inventario.API.Controllers;
[Authorize]
[ApiController]
[Route("api")]
public class ReporteController : ControllerBase
{
    private readonly IReporteInventarioService _reporteInventarioService;
    private readonly IPdfReporteService _pdfReporteService;
    public ReporteController (IReporteInventarioService reporteInventarioService,
                                IPdfReporteService pdfReporteService)
    {
        _reporteInventarioService = reporteInventarioService;
        _pdfReporteService = pdfReporteService;
    }

    [Authorize]
    [HttpGet("reportes")]
    public async Task<IActionResult> Inventario ()
    {
        var reporte = await _reporteInventarioService.ObtenerReporteAsync();
        return Ok(reporte);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("pdf")]
    public async Task<IActionResult> GenerarPdf()
    {
        var reporte = await _reporteInventarioService.ObtenerReporteAsync();

        var pdf = _pdfReporteService.GenerarPdf(reporte);

        return File(pdf,"Application/pdf", "ReporteInventario.pdf");
    }
}

