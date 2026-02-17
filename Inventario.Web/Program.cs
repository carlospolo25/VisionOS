using Inventario.Web.filters;
using Inventario.Web.Handlers;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

using System;
using System.Text;
using System.Text.Json;

namespace Inventario.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<JwtSessionFilter>();
            // Add services to the container.

            builder.Services.AddHttpClient();
            builder.Services.AddSession();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<JwtAuthorizationHandler>();

            builder.Services.AddHttpClient("ApiClient", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5225/");
            })
            .AddHttpMessageHandler<JwtAuthorizationHandler>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth/Login";
                    options.AccessDeniedPath = "/Auth/AccessDenied";

                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

                    options.SlidingExpiration = true;
                });

            builder.Services.AddControllersWithViews(options =>
            {
                // 🔐 Filtro global: TODO requiere autenticación
                options.Filters.Add(new AuthorizeFilter());
            })
            .AddApplicationPart(typeof(Inventario.Web.Controllers.AuthController).Assembly);


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
