using Gestion_de_productos.Data.Context;
using Gestion_de_productos.Shared.Entities;

namespace Gestion_de_productos.Data.Repositories
{
    public class CarritoItemRepository : ICarritoItemRepository
    {
        private readonly AppDbContext _context;

        public CarritoItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AgregarAsync(CarritoItem item)
        {
            await _context.CarritoItems.AddAsync(item);
        }

        public void Actualizar(CarritoItem item)
        {
            _context.CarritoItems.Update(item);
        }

        public void Eliminar(CarritoItem item)
        {
            _context.CarritoItems.Remove(item);
        }

        public void EliminarRango(IEnumerable<CarritoItem> items)
        {
            _context.CarritoItems.RemoveRange(items);
        }
    }
}