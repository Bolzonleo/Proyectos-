using Gestion_de_productos.Data;
using Gestion_de_productos.DTOs;
using Gestion_de_productos.Models;
using Gestion_de_productos.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gestion_de_productos.Services
{
    public class PedidoDetalleService : IPedidoDetalleService
    {
        private readonly AppDbContext _context;

        public PedidoDetalleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PedidoDetalleDTO>> ObtenerPorPedidoIdAsync(int pedidoId)
        {
            var detalles = await _context.PedidoDetalles
                .Include(d => d.Producto)
                .Where(d => d.PedidoId == pedidoId)
                .ToListAsync();

            return detalles.Select(Mapear).ToList();
        }

        public async Task<PedidoDetalleDTO> AgregarDetalleAsync(int pedidoId, CrearPedidoDetalleDTO dto)
        {
            if (dto.Cantidad <= 0)
                throw new Exception("La cantidad debe ser mayor a 0");

            var pedido = await _context.Pedidos.FirstOrDefaultAsync(p => p.Id == pedidoId);
            if (pedido == null)
                throw new Exception("Pedido no encontrado");

            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.Id == dto.ProductoId);
            if (producto == null)
                throw new Exception("Producto no encontrado");

            var detalle = new PedidoDetalle
            {
                PedidoId = pedidoId,
                ProductoId = dto.ProductoId,
                Cantidad = dto.Cantidad,
                PrecioUnitario = dto.PrecioUnitario > 0 ? (double)dto.PrecioUnitario : (double)producto.Precio
            };

            _context.PedidoDetalles.Add(detalle);
            pedido.Total += detalle.PrecioUnitario * detalle.Cantidad;
            await _context.SaveChangesAsync();

            detalle.Producto = producto;
            return Mapear(detalle);
        }

        public async Task<bool> EliminarDetalleAsync(int id)
        {
            var detalle = await _context.PedidoDetalles.FirstOrDefaultAsync(d => d.Id == id);
            if (detalle == null)
                throw new Exception("Detalle no encontrado");

            var pedido = await _context.Pedidos.FirstOrDefaultAsync(p => p.Id == detalle.PedidoId);
            if (pedido != null)
            {
                pedido.Total -= detalle.PrecioUnitario * detalle.Cantidad;
                if (pedido.Total < 0)
                    pedido.Total = 0;
            }

            _context.PedidoDetalles.Remove(detalle);
            await _context.SaveChangesAsync();
            return true;
        }

        private static PedidoDetalleDTO Mapear(PedidoDetalle d)
        {
            return new PedidoDetalleDTO
            {
                Id = d.Id,
                PedidoId = d.PedidoId,
                ProductoId = d.ProductoId,
                Cantidad = d.Cantidad,
                PrecioUnitario = (decimal)d.PrecioUnitario,
                ProductoNombre = d.Producto?.Nombre ?? string.Empty
            };
        }
    }
}
