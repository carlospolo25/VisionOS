using Inventario.Web.Models.Auth;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace Inventario.Web.Controllers;

public class AuthController : Controller
{
    public readonly IHttpClientFactory _http;

    public AuthController(IHttpClientFactory http)
    {
        _http = http;
    }

    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult AccessDenied(string returnUrl)
    {
        if (!Url.IsLocalUrl(returnUrl))
            returnUrl = "/Dashboard";

        ViewBag.ReturnUrl = returnUrl;
        return View();
    }



    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(loginViewModel model)
    {
        var client = _http.CreateClient();

        var json = JsonSerializer.Serialize(model);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(
            "http://localhost:5225/api/auth/login", content);

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.error = "Credenciales inválidas";
            return View();
        }

        // 🔹 Leer respuesta
        var result = await response.Content.ReadAsStringAsync();    
        var tokenObj = JsonSerializer.Deserialize<JsonElement>(result);

        var token = tokenObj.GetProperty("token").GetString();

        // 🔐 Guardar JWT (para API)
        HttpContext.Session.SetString("JWT", token);

        // 🔑 CREAR AUTENTICACIÓN MVC 
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, model.Email),
            new Claim(ClaimTypes.Role, "Admin"),
        };

        var identity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme
            );

        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal
            );

        return RedirectToAction("Dashboard","Dashboard");

    }
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Register (RegisterViewModel model)
    {
        var client = _http.CreateClient();
        var json = JsonSerializer.Serialize(model);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("http://localhost:5225/api/auth/register", content);
        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "No se pudo registrar.";
            return View("Login");
        }

        return RedirectToAction("Login");

    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme
            );

        HttpContext.Session.Clear();

        return RedirectToAction("login");
    }


}

