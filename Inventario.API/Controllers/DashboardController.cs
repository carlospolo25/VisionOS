using Microsoft.AspNetCore.Http;
using Inventario.API.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Inventario.API.Application.Services.Dashboard;
using Microsoft.AspNetCore.Authorization;

namespace Inventario.API.Controllers;
[Authorize]
[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _service;

    public DashboardController(IDashboardService dashboardService)
    {
        _service = dashboardService;
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var resumen = await _service.Productos();
        return Ok(resumen);
    }

}
