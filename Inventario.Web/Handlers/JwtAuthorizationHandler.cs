using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Inventario.Web.Handlers;

public class JwtAuthorizationHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthenticationService _authService;

    public JwtAuthorizationHandler(IHttpContextAccessor httpContextAccessor,
        IAuthenticationService authService)
    {
        _httpContextAccessor = httpContextAccessor;
        _authService = authService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
     HttpRequestMessage request,
     CancellationToken cancellationToken)
    {
        var context = _httpContextAccessor.HttpContext;

        var token = context?.Session.GetString("JWT");

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await base.SendAsync(request, cancellationToken);

        // 🚫 Usuario autenticado pero sin permisos (ROL)
        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            context!.Response.Redirect("/Auth/AccessDenied");
            return response;
        }


        // 🔁 JWT expirado → intentar refresh
        if (response.StatusCode == HttpStatusCode.Unauthorized && token != null)
        {
            var refreshRequest = new HttpRequestMessage(
                HttpMethod.Post, "api/auth/refresh");

            refreshRequest.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var refreshResponse =
                await base.SendAsync(refreshRequest, cancellationToken);

            // ❌ Refresh falló → logout
            if (!refreshResponse.IsSuccessStatusCode)
            {
                context!.Session.Clear();

                await _authService.SignOutAsync(
                    context,
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    null);

                return response; // deja que MVC redirija a Login
            }

            // ✅ Refresh OK
            var json = await refreshResponse.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            var newToken = doc.RootElement.GetProperty("token").GetString();

            context!.Session.SetString("JWT", newToken!);

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", newToken);

            return await base.SendAsync(request, cancellationToken);


        }

        return response;

    }


}
