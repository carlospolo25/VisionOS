using Inventario.API.Domain.Entities;
using Inventario.API.Infrastructure.Data;
using Inventario.API.Migrations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Inventario.API.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Producto> Producto => Set<Producto>();  
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rol>().HasData(
            new Rol { Id = 1, Nombre = "Admin" },
            new Rol { Id = 2, Nombre = "Usuario" }
        );

        modelBuilder.Entity<Producto>()
            .Property(P => P.Precio)
            .HasPrecision(18, 2);
    }

}
