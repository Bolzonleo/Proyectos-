using Gestion_de_productos.Data.Context;
using Gestion_de_productos.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gestion_de_productos.Data.Repositories
{
    public class CarritoRepository : ICarritoRepository
    {
        private readonly AppDbContext _context;

        public CarritoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Carrito?> ObtenerPorUsuarioIdAsync(int usuarioId)
        {
            return await _context.Carritos
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);
        }

        public async Task<Carrito?> ObtenerConItemsAsync(int usuarioId)
        {
            return await _context.Carritos
                .Include(c => c.Items)
                .ThenInclude(i => i.Producto)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);
        }

        public async Task CrearAsync(Carrito carrito)
        {
            _context.Carritos.Add(carrito);
            await _context.SaveChangesAsync();
        }

        public async Task GuardarCambiosAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}