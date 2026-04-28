using Gestion_de_productos.Data.Context;
using Gestion_de_productos.Models;
using Gestion_de_productos.Services.Interfaces;
using Gestion_de_productos.Shared.DTOs;
using Gestion_de_productos.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gestion_de_productos.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly AppDbContext _context;

        public PedidoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PedidoDTO>> ObtenerTodosAsync()
        {
            var pedidos = await _context.Pedidos.ToListAsync();
            return pedidos.Select(Mapear).ToList();
        }

        public async Task<PedidoDTO> ObtenerPorIdAsync(int id)
        {
            var pedido = await _context.Pedidos.FirstOrDefaultAsync(p => p.Id == id);
            if (pedido == null)
                throw new Exception("Pedido no encontrado");

            return Mapear(pedido);
        }

        public async Task<PedidoDTO> CrearAsync(CrearPedidoDTO dto)
        {
            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.Id == dto.UsuarioId);
            if (!usuarioExiste)
                throw new Exception("El usuario no existe");

            var pedido = new Pedido
            {
                UsuarioId = dto.UsuarioId,
                Estado = string.IsNullOrWhiteSpace(dto.Estado) ? "Pendiente" : dto.Estado,
                Fecha = DateTime.UtcNow,
                Total = 0
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return Mapear(pedido);
        }

        public async Task<PedidoDTO> CrearDesdeCarritoAsync(int usuarioId)
        {
            var carrito = await _context.Carritos
                .Include(c => c.Items)
                .ThenInclude(i => i.Producto)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

            if (carrito == null || !carrito.Items.Any())
                throw new Exception("El carrito está vacío");

            var pedido = new Pedido
            {
                UsuarioId = usuarioId,
                Fecha = DateTime.UtcNow,
                Estado = "Pendiente",
                Total = carrito.Items.Sum(i => (double)(i.Producto.Precio * i.Cantidad)),
                Detalles = carrito.Items.Select(i => new PedidoDetalle
                {
                    ProductoId = i.ProductoId,
                    Cantidad = i.Cantidad,
                    PrecioUnitario = (double)i.Producto.Precio
                }).ToList()
            };

            _context.Pedidos.Add(pedido);
            _context.CarritoItems.RemoveRange(carrito.Items);
            await _context.SaveChangesAsync();

            return Mapear(pedido);
        }

        public async Task<bool> ActualizarEstadoAsync(int id, ActualizarPedidoDTO dto)
        {
            var pedido = await _context.Pedidos.FirstOrDefaultAsync(p => p.Id == id);
            if (pedido == null)
                throw new Exception("Pedido no encontrado");

            if (string.IsNullOrWhiteSpace(dto.Estado))
                throw new Exception("El estado es obligatorio");

            pedido.Estado = dto.Estado.Trim();
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var pedido = await _context.Pedidos.FirstOrDefaultAsync(p => p.Id == id);
            if (pedido == null)
                throw new Exception("Pedido no encontrado");

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();
            return true;
        }

        private static PedidoDTO Mapear(Pedido pedido)
        {
            return new PedidoDTO
            {
                Id = pedido.Id,
                UsuarioId = pedido.UsuarioId,
                Fecha = pedido.Fecha,
                Total = pedido.Total,
                Estado = pedido.Estado
            };
        }
    }
}
