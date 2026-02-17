using Azure;
using Inventario.Web.Models.Gestion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace Inventario.Web.Controllers;

[Authorize(Roles ="Admin")]
public class GestionController : Controller
{
    private readonly IHttpClientFactory _http;

    public GestionController(IHttpClientFactory http)
    {
        _http = http;
    }


    // Cargar partial (GET)
    [HttpGet]
    public IActionResult Crear()
    {
        return View("Crear");
    }
    [Authorize(Roles ="Admin")]
    [HttpGet]
    public async Task<IActionResult> Gestion (string busqueda = "")
    {
        var client = _http.CreateClient("ApiClient");

        var responce = await client.GetAsync(
            $"http://localhost:5225/api/gestion/producto?busqueda={busqueda}");

        var productos = new List<GestionViewModel>();

        if (responce.IsSuccessStatusCode)
        {
            productos = await responce.Content
                .ReadFromJsonAsync<List<GestionViewModel>> ()
                ?? new List<GestionViewModel> ();
        }

        var model = new EditarGestionViewModel
        {
            Productos = productos,
            Busqueda = busqueda
        };

        return View(model);
    }


    // Recibir formulario (POST)
    [HttpPost]
    public async Task<IActionResult> Crear(GestionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Crear", model);
        }

        var client = _http.CreateClient("ApiClient");

        var response = await client.PostAsJsonAsync("api/gestion/crear", model
        );
        // 🔐 No autorizado → sesión expirada o no logueado
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            TempData["Error"] = "Tu sesión ha expirado.";
            return RedirectToAction("Login", "Auth");
        }

        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            TempData["Error"] = "No tienes permisos para ver el dashboard.";
            return RedirectToAction("login", "Auth");
        }

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", "Error al crear el producto");
            return View("Crear", model);
        }

        TempData["Success"] = "Producto creado correctamente.";
        return RedirectToAction("Gestion");
    }

    [HttpGet]
    public async Task<IActionResult> Editar(string busqueda = "")
    {
        var client = _http.CreateClient("ApiClient");
        var url = "http://localhost:5225/api/gestion/producto";
       
        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            url += $"?busqueda={busqueda}";
        }

        var response = await client.GetAsync(url);

        var productos = new List<GestionViewModel>();

        if (response.IsSuccessStatusCode)
        {
            productos = await response.Content
                .ReadFromJsonAsync<List<GestionViewModel>>();
        }

        var model = new EditarGestionViewModel
        {
            Productos = productos,
            ProductoEditar = new GestionViewModel(), // 🔥 CLAVE
            Busqueda = busqueda
        };

        return View(model); // ✅ AQUÍ ESTABA TODO
    }

    [HttpPost]
    public async Task<IActionResult> Editar(GestionViewModel model)
    {
        var client = _http.CreateClient("ApiClient");

        var response = await client.PutAsJsonAsync(
            $"http://localhost:5225/api/gestion/{model.Id}/editar",
            model
        );

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Error al actualizar el producto";
            return RedirectToAction(nameof(Editar));
        }

        TempData["Success"] = "Producto actualizado correctamente";
        return RedirectToAction(nameof(Editar));
    }


    [HttpGet]
    public async Task<IActionResult> Eliminar(string? q)
    {
        var client = _http.CreateClient("ApiClient");

        var response = await client.GetAsync(
            "http://localhost:5225/api/gestion/Producto"
            );

        if (!response.IsSuccessStatusCode)
        {
            return RedirectToAction("Gestion");
        }

        var productos = await response.Content
            .ReadFromJsonAsync<List<GestionViewModel>>()
            ?? new List<GestionViewModel>();


        if (!string.IsNullOrWhiteSpace(q))
        {
            productos = productos
                .Where(p =>
                p.Nombre.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                p.Descripcion.Contains(q, StringComparison.OrdinalIgnoreCase)
                
                )
                .ToList();
        }

        return View("Eliminar", productos);
    }

    [HttpPost]
    public async Task<IActionResult> Eliminar(int id)
    {
        var client = _http.CreateClient("ApiClient");

        var response = await client.DeleteAsync(
            $"http://localhost:5225/api/gestion/{id}/eliminar"
            );

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "No se pudo eliminar el producto.";
            return RedirectToAction("Eliminar");
        }

        TempData["Success"] = "Pruducto eliminado correctamente.";
        return RedirectToAction("Eliminar");

    }

    [HttpGet]
    public async Task<IActionResult> Estado(string? e)
    {
        var client = _http.CreateClient("ApiClient");

        var response = await client.GetAsync(
            "http://localhost:5225/api/gestion/producto"
            );

        if (!response.IsSuccessStatusCode)
        {
            return RedirectToAction("Gestion");
        }
        var productos = await response.Content
        .ReadFromJsonAsync<List<GestionViewModel>>()
        ?? new List<GestionViewModel>();

        if (!string.IsNullOrWhiteSpace(e))
        {
            productos = productos
                .Where(p =>
                p.Nombre.Contains(e, StringComparison.OrdinalIgnoreCase) ||
                p.Descripcion.Contains(e, StringComparison.OrdinalIgnoreCase)

                )
                .ToList();
        }


        return View("Estado", productos);
    }

    [HttpPost]
    public async Task<IActionResult> Estado(int id)
    {
        Console.WriteLine("El producto no se activo ");
        var client = _http.CreateClient("ApiClient");

        var request = new HttpRequestMessage(
            HttpMethod.Patch,
            $"api/gestion/{id}/activar"
            );    
           
        var response = await client.SendAsync(request);

        Console.WriteLine("el Producto Se Activo");

        if (!response.IsSuccessStatusCode)
            TempData["Error"] = "No se puede activar el producto.";

        return RedirectToAction("Estado");
    }

    [HttpPost]
    public async Task<IActionResult> Desactivar(int id)
    {                                
        var client = _http.CreateClient("ApiClient");

        var request = new HttpRequestMessage(
        HttpMethod.Patch,
        $"api/gestion/{id}/desactivar"
        );

        var response = await client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            TempData["Error"] = "No se pudo activar elproducto. ";

        return RedirectToAction("Estado");
    }


}

