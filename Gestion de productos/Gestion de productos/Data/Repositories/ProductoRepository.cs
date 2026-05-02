using Gestion_de_productos.Data.Context;
using Gestion_de_productos.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gestion_de_productos.Data.Repositories;

public class ProductoRepository
: IProductoRepository
{
    private readonly AppDbContext _context;

    public ProductoRepository(
        AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Producto>>
       ObtenerTodosAsync()
    {
        return await _context.Productos
           .Include(p => p.Categoria)
           .ToListAsync();
    }

    public async Task<Producto?>
      ObtenerPorIdAsync(int id)
    {
        return await _context.Productos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(
               p => p.Id == id);
    }

    public async Task<List<Producto>>
      ObtenerPorCategoriaAsync(
         int categoriaId)
    {
        return await _context.Productos
           .Include(p => p.Categoria)
           .Where(
             p => p.CategoriaId == categoriaId)
           .ToListAsync();
    }

    public async Task<List<Producto>>
      BuscarPorNombreAsync(
       string termino)
    {
        return await _context.Productos
            .Include(p => p.Categoria)
            .Where(
            p => EF.Functions.Like(
            p.Nombre,
            $"%{termino}%"))
            .ToListAsync();
    }

    public async Task CrearAsync(
       Producto producto)
    {
        _context.Productos.Add(producto);

        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(
      Producto producto)
    {
        _context.Productos.Update(producto);

        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(
       Producto producto)
    {
        _context.Productos.Remove(producto);

        await _context.SaveChangesAsync();
    }

    public async Task<bool>
       ExisteCategoriaAsync(
          int categoriaId)
    {
        return await _context.Categorias
           .AnyAsync(c => c.Id == categoriaId);
    }
}
