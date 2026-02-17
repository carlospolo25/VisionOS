using Inventario.API.Application.Services.Productos;
using Inventario.API.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/productos")]
public class ProductosController : ControllerBase
{
    private readonly IProductosService _productosService;

    public ProductosController(IProductosService productosService)
    {
        _productosService = productosService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var productos = await _productosService.ObtenerTodoAsync();
        return Ok(productos);
    }
}
