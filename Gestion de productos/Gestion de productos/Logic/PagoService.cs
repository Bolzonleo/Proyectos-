using Gestion_de_productos.Data.Context;
using Gestion_de_productos.Services.Interfaces;
using Gestion_de_productos.Shared.DTOs;
using Gestion_de_productos.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gestion_de_productos.Services
{
    public class PagoService : IPagoService
    {
        private readonly AppDbContext _context;

        public PagoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PagoDTO>> ObtenerTodosAsync()
        {
            var pagos = await _context.Pagos.ToListAsync();
            return pagos.Select(Mapear).ToList();
        }

        public async Task<PagoDTO> ObtenerPorIdAsync(int id)
        {
            var pago = await _context.Pagos.FirstOrDefaultAsync(p => p.Id == id);
            if (pago == null)
                throw new Exception("Pago no encontrado");

            return Mapear(pago);
        }

        public async Task<PagoDTO> CrearAsync(CrearPagoDTO dto)
        {
            if (dto.Monto <= 0)
                throw new Exception("El monto debe ser mayor a 0");

            var pedidoExiste = await _context.Pedidos.AnyAsync(p => p.Id == dto.PedidoId);
            if (!pedidoExiste)
                throw new Exception("Pedido no encontrado");

            var yaExistePago = await _context.Pagos.AnyAsync(p => p.PedidoId == dto.PedidoId);
            if (yaExistePago)
                throw new Exception("El pedido ya tiene un pago registrado");

            var pago = new Pago
            {
                PedidoId = dto.PedidoId,
                Monto = (double)dto.Monto,
                FormaPago = dto.FormaPago,
                Estado = "Pendiente",
                FechaPago = DateTime.UtcNow
            };

            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();
            return Mapear(pago);
        }

        public async Task<bool> ActualizarEstadoAsync(int id, ActualizarPagoDTO dto)
        {
            var pago = await _context.Pagos.FirstOrDefaultAsync(p => p.Id == id);
            if (pago == null)
                throw new Exception("Pago no encontrado");
            if (string.IsNullOrWhiteSpace(dto.Estado))
                throw new Exception("Estado inválido");

            pago.Estado = dto.Estado.Trim();
            _context.Pagos.Update(pago);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var pago = await _context.Pagos.FirstOrDefaultAsync(p => p.Id == id);
            if (pago == null)
                throw new Exception("Pago no encontrado");

            _context.Pagos.Remove(pago);
            await _context.SaveChangesAsync();
            return true;
        }

        private static PagoDTO Mapear(Pago pago)
        {
            return new PagoDTO
            {
                Id = pago.Id,
                PedidoId = pago.PedidoId,
                Monto = (decimal)pago.Monto,
                FormaPago = pago.FormaPago,
                Estado = pago.Estado,
                FechaPago = pago.FechaPago
            };
        }
    }
}
