using Inventario.Web.filters;
using Inventario.Web.Models.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;




namespace Inventario.Web.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly IHttpClientFactory _http;

    public DashboardController (IHttpClientFactory http)
    {
        _http = http;
    }
    public async Task<IActionResult> Dashboard()
    {
        var client = _http.CreateClient("ApiClient");

        var response = await client.GetAsync("api/dashboard");


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


        // ❌ Otro error
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "No se puede cargar el Dashboard";
            return View(new DashboardViewModel());
        }


        // ✅ Todo OK
        var dashboard = await response.Content
            .ReadFromJsonAsync<DashboardViewModel>();

        return View(dashboard ?? new DashboardViewModel());
    }

}

