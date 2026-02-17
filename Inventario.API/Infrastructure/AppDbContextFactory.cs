using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Inventario.API.Infrastructure.Data;

namespace Inventario.API.Infrastructure;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // Conexión a tu SQL Server
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=InventarioDB;Trusted_Connection=True;TrustServerCertificate=True");

        return new AppDbContext(optionsBuilder.Options);
    }
}
