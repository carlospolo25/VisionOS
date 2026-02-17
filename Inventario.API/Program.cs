
using Inventario.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Inventario.API.Application.Services.Auth;
using Inventario.API.Application.Services.Dashboard;
using Inventario.API.Application.Services.Productos;
using Inventario.API.Application.Services.Gestion;
using Inventario.API.Application.Services.Reportes;
using QuestPDF.Infrastructure;

namespace Inventario.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddScoped<IAuthService, AuthService>();
            Console.WriteLine(">>> API STARTED - AUTH SERVICE REGISTERED <<<");
            builder.Services.AddScoped<IDashboardService, DashdoardService>();
            builder.Services.AddScoped<IProductosService, ProductosService>();
            builder.Services.AddScoped<IProductoQueryService, ProductoQueryService>();
            builder.Services.AddScoped<IGestionService, GestionService>();
            builder.Services.AddScoped<IProductoReadService, ProductoReadService>();
            builder.Services.AddScoped<IReporteInventarioService, ReporteInventarioServicecs>();
            builder.Services.AddScoped<IPdfReporteService, PdfReporteService>();

            QuestPDF.Settings.License = LicenseType.Community;
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
            })
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
                    ),

                    ClockSkew = TimeSpan.Zero
                };
            });


            Console.WriteLine(">>> API STARTED - AUTH SERVICE REGISTERED <<<");
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
