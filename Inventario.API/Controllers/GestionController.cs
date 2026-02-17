using Inventario.API.Application.DTOs.Gestion;
using Inventario.API.Application.Services.Gestion;
using Inventario.API.Application.Services.Productos;
using Inventario.API.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.API.Controllers;
[Authorize(Roles = "Admin")]
[Route("api/gestion")]
[ApiController]
public class GestionController : ControllerBase
{
    private readonly IGestionService _gestionService;
    private readonly IProductosService _productosService;
    private readonly IProductoReadService _productoReadService;


    public GestionController(IGestionService gestionService, 
                             IProductosService productosService,
                             IProductoReadService productoReadService)
    {
        _gestionService = gestionService;
        _productosService = productosService;
        _productoReadService = productoReadService;
    }

    // ======================
    // 📦 LISTAR PRODUCTOS
    // ======================
    [Authorize(Roles = "Admin")]
    [HttpGet("producto")]
    public async Task<IActionResult> ObtenerTodos([FromQuery] string busqueda = "")
    {
        var productos = await _productosService.ObtenerTodoAsync(busqueda);
        return Ok(productos);
    }


    // ======================
    // ➕ CREAR PRODUCTO
    // ======================
    [Authorize(Roles ="Admin")]
    [HttpPost("crear")]
    public async Task<IActionResult> CreaProductosAsync([FromBody] CrearProductosDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var id = await _gestionService.CrearProductosAsync(dto);
        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id },
            new { id }
            );

    }

    // ======================
    // 🔍 OBTENER POR ID
    // ======================
    [Authorize(Roles = "Admin")]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var producto = await _productoReadService.ObtenerPorIdAsync(id);
        return Ok(producto);
    }

    // ======================
    // ✏ EDITAR PRODUCTO
    // ======================
    [Authorize(Roles ="Admin")]
    [HttpPut("{id:int}/editar")]
    public async Task<IActionResult> EditarProductosAsync(int id, [FromBody] EditarProductosDto dto)
    {
        var ok = await _gestionService.EditarProductosAsync(id, dto);
        if (!ok)
            return NotFound();

        return NoContent();
    }

    // ======================
    // 🗑 ELIMINAR PRODUCTO
    // ======================
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}/eliminar")]
    public async Task<IActionResult> EliminarProductosAsync (int id)
    {
        var ok = await _gestionService.EliminarProductosAsync(id);

        if (!ok)
            return NotFound();

        return NoContent() ;    
    }

    // ======================
    // 🔄 ACTIVAR
    // ======================
    [Authorize(Roles = "Admin")]
    [HttpPatch("{id:int}/activar")]
    public async Task<IActionResult> ActivarProductosAsync (int id)
    {
        var ok = await _gestionService.ActivarProductosAsync(id);

            if(!ok)
                return NotFound();

        return NoContent();
    }

    // ======================
    // ⛔ DESACTIVAR
    // ======================
    [Authorize(Roles = "Admin")]
    [HttpPatch("{id:int}/desactivar")]
    public async Task<IActionResult> DesactivarProductosAsync (int id)
    {
        var ok = await _gestionService.DesactivarProductosAsync(id);

        if (!ok)
            return NotFound();

        return NoContent();
    }

}

