using Gestion_de_productos.Data.Context;
using Gestion_de_productos.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gestion_de_productos.Data.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Categoria>> ObtenerTodosAsync()
        {
            return await _context.Categorias
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Categoria?> ObtenerPorIdAsync(int id)
        {
            return await _context.Categorias
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> ExistePorNombreAsync(string nombre, int? excluirId = null)
        {
            var nombreNormalizado = nombre.Trim().ToLower();

            return await _context.Categorias
                .AnyAsync(c =>
                    c.Nombre.ToLower() == nombreNormalizado &&
                    (!excluirId.HasValue || c.Id != excluirId.Value));
        }

        public async Task CrearAsync(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Categoria categoria)
        {
            _context.Categorias.Update(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(Categoria categoria)
        {
            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> TieneProductosAsync(int categoriaId)
        {
            return await _context.Productos
                .AnyAsync(p => p.CategoriaId == categoriaId);
        }
    }
}