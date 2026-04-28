using Gestion_de_productos.Data.Context;
using Gestion_de_productos.Services.Interfaces;
using Gestion_de_productos.Shared.DTOs;
using Gestion_de_productos.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gestion_de_productos.Services
{
    public class EnvioService : IEnvioService
    {
        private readonly AppDbContext _context;

        public EnvioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EnvioDTO>> ObtenerTodosAsync()
        {
            var envios = await _context.Envios.ToListAsync();
            return envios.Select(Mapear).ToList();
        }

        public async Task<EnvioDTO> ObtenerPorIdAsync(int id)
        {
            var envio = await _context.Envios.FirstOrDefaultAsync(e => e.Id == id);
            if (envio == null)
                throw new Exception("Envío no encontrado");

            return Mapear(envio);
        }

        public async Task<EnvioDTO> CrearAsync(CrearEnvioDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Direccion))
                throw new Exception("La dirección es obligatoria");

            var pedidoExiste = await _context.Pedidos.AnyAsync(p => p.Id == dto.PedidoId);
            if (!pedidoExiste)
                throw new Exception("Pedido no encontrado");

            var envioExistente = await _context.Envios.AnyAsync(e => e.PedidoId == dto.PedidoId);
            if (envioExistente)
                throw new Exception("El pedido ya tiene un envío creado");

            var envio = new Envio
            {
                PedidoId = dto.PedidoId,
                Direccion = dto.Direccion.Trim(),
                Estado = "Pendiente",
                FechaEnvio = DateTime.UtcNow
            };

            _context.Envios.Add(envio);
            await _context.SaveChangesAsync();
            return Mapear(envio);
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarEnvioDTO dto)
        {
            var envio = await _context.Envios.FirstOrDefaultAsync(e => e.Id == id);
            if (envio == null)
                throw new Exception("Envío no encontrado");

            if (!string.IsNullOrWhiteSpace(dto.Estado))
                envio.Estado = dto.Estado.Trim();

            if (dto.FechaEntrega.HasValue)
                envio.FechaEntrega = dto.FechaEntrega.Value;

            _context.Envios.Update(envio);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var envio = await _context.Envios.FirstOrDefaultAsync(e => e.Id == id);
            if (envio == null)
                throw new Exception("Envío no encontrado");

            _context.Envios.Remove(envio);
            await _context.SaveChangesAsync();
            return true;
        }

        private static EnvioDTO Mapear(Envio envio)
        {
            return new EnvioDTO
            {
                Id = envio.Id,
                PedidoId = envio.PedidoId,
                Direccion = envio.Direccion,
                Estado = envio.Estado,
                FechaEnvio = envio.FechaEnvio,
                FechaEntrega = envio.FechaEntrega == default ? null : envio.FechaEntrega
            };
        }
    }
}
