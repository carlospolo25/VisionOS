using Inventario.API.Application.DTOs.Gestion;
using Inventario.API.Infrastructure.Data;
using Inventario.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Azure.Messaging;


namespace Inventario.API.Application.Services.Gestion;

public class GestionService : IGestionService 
{
    private readonly AppDbContext _context;

    public GestionService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<int> CrearProductosAsync(CrearProductosDto dto)
    {
        var existe = await _context.Producto
            .AnyAsync(p => p.Nombre == dto.Nombre);

        if (existe)
            throw new Exception("El producto ya existe ");

        var producto = new Producto
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            Precio = dto.Precio,
            Stock = dto.Stock,
        };
        _context.Producto.Add(producto);
        await _context.SaveChangesAsync();

        return producto.Id;
    }

    public async Task<bool> EditarProductosAsync(int id, EditarProductosDto dto)
    {
        var producto = await _context.Producto.FindAsync(id);

        if(producto == null)
            return false;

        producto.Nombre = dto.Nombre ;
        producto.Descripcion = dto.Descripcion;
        producto.Precio = dto.Precio;
        producto.Stock = dto.Stock ?? 0;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EliminarProductosAsync (int id)
    {
        var productos = await _context.Producto.FindAsync (id);

        if (productos == null)
            return false;

        _context.Producto.Remove(productos);
        await _context.SaveChangesAsync();
        return true;

    }

    public async Task<bool> ActivarProductosAsync (int id)
    {
        var productos = await _context.Producto.FindAsync(id);

        if (productos == null)
            return false;

        if (productos.Activo)
            return false;

        productos.Activo = true;

        await _context.SaveChangesAsync();
        return true;
        
    }

    public async Task<bool> DesactivarProductosAsync (int id)
    {
        var productos = await _context.Producto.FindAsync(id);

        if (productos == null)
        return false;

        if (!productos.Activo)
        return false;

        productos.Activo = false;

        await _context.SaveChangesAsync();
        return true;
        
    }

}

