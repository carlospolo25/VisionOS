using Inventario.Web.filters;
using Inventario.Web.Models.Dashboard;
using Inventario.Web.Models.Reportes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Inventario.Web.Controllers;

public class ReportesController : Controller
{
    private readonly IHttpClientFactory _http;

    public ReportesController (IHttpClientFactory http)
    {
        _http = http;
    }

    [HttpGet]
    public async Task<IActionResult> Reportes ()
    {
        var client = _http.CreateClient("ApiClient");

        var response = await client.GetAsync("api/reportes");

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "No se puede cargar el Dashboard";
            return View(new ReportesViewModel());
        }

        var reportes = await response.Content
            .ReadFromJsonAsync<ReportesViewModel>();

        Console.WriteLine("Reportes");
        Console.WriteLine(reportes?.TotalProductos );
        Console.WriteLine("Termino Reporte");

        return View(reportes ?? new ReportesViewModel());

    }

    [HttpGet]
    public async Task<IActionResult> DescargarPdf()
    {
        var client = _http.CreateClient("ApiClient");
        var response = await client.GetAsync("api/pdf");
        if (!response.IsSuccessStatusCode)
            return RedirectToAction("Reportes");

        var pdf = await response.Content.ReadAsByteArrayAsync();

        return File(pdf, "Application/pdf", "ReporteInventario.pdf");
    }


}

