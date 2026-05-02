using Gestion_de_productos.Data.Context;
using Gestion_de_productos.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gestion_de_productos.Data.Repositories
{
    public class PedidoDetalleRepository : IPedidoDetalleRepository
    {
        private readonly AppDbContext _context;

        public PedidoDetalleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PedidoDetalle>> ObtenerPorPedidoIdAsync(int pedidoId)
        {
            return await _context.PedidoDetalles
                .AsNoTracking()
                .Include(d => d.Producto)
                .Where(d => d.PedidoId == pedidoId)
                .ToListAsync();
        }

        public async Task<PedidoDetalle?> ObtenerPorIdAsync(int id)
        {
            return await _context.PedidoDetalles
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task AgregarAsync(PedidoDetalle detalle)
        {
            await _context.PedidoDetalles.AddAsync(detalle);
        }

        public void Eliminar(PedidoDetalle detalle)
        {
            _context.PedidoDetalles.Remove(detalle);
        }
        public async Task AgregarRangoAsync(IEnumerable<PedidoDetalle> detalles)
        {
            await _context.PedidoDetalles.AddRangeAsync(detalles);
        }
    }
}