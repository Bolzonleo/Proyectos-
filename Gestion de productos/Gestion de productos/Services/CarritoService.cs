using Gestion_de_productos.Data;
using Gestion_de_productos.DTOs;
using Gestion_de_productos.Models;
using Gestion_de_productos.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gestion_de_productos.Services
{
    public class CarritoService : ICarritoService
    {
        private readonly AppDbContext _context;

        public CarritoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CarritoDTO> ObtenerPorUsuarioIdAsync(int usuarioId)
        {
            var carrito = await _context.Carritos
                .Include(c => c.Items)
                .ThenInclude(i => i.Producto)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

            if (carrito == null)
            {
                await CrearParaUsuarioAsync(usuarioId);
                carrito = await _context.Carritos
                    .Include(c => c.Items)
                    .ThenInclude(i => i.Producto)
                    .FirstAsync(c => c.UsuarioId == usuarioId);
            }

            return Mapear(carrito);
        }

        public async Task<CarritoDTO> CrearParaUsuarioAsync(int usuarioId)
        {
            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.Id == usuarioId);
            if (!usuarioExiste)
                throw new Exception("El usuario no existe");

            var carritoExistente = await _context.Carritos.FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);
            if (carritoExistente != null)
                return Mapear(carritoExistente);

            var carrito = new Carrito { UsuarioId = usuarioId };
            _context.Carritos.Add(carrito);
            await _context.SaveChangesAsync();

            return Mapear(carrito);
        }

        public async Task<CarritoDTO> AgregarItemAsync(int usuarioId, AgregarAlCarritoDTO dto)
        {
            if (dto.Cantidad <= 0)
                throw new Exception("La cantidad debe ser mayor a 0");

            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.Id == dto.ProductoId);
            if (producto == null)
                throw new Exception("El producto no existe");

            var carrito = await _context.Carritos
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

            if (carrito == null)
            {
                await CrearParaUsuarioAsync(usuarioId);
                carrito = await _context.Carritos.Include(c => c.Items).FirstAsync(c => c.UsuarioId == usuarioId);
            }

            var item = carrito.Items.FirstOrDefault(i => i.ProductoId == dto.ProductoId);
            if (item == null)
            {
                item = new CarritoItem
                {
                    CarritoId = carrito.Id,
                    ProductoId = dto.ProductoId,
                    Cantidad = dto.Cantidad
                };
                _context.CarritoItems.Add(item);
            }
            else
            {
                item.Cantidad += dto.Cantidad;
                _context.CarritoItems.Update(item);
            }

            await _context.SaveChangesAsync();
            return await ObtenerPorUsuarioIdAsync(usuarioId);
        }

        public async Task<CarritoDTO> ActualizarCantidadAsync(int usuarioId, int productoId, int cantidad)
        {
            if (cantidad <= 0)
                throw new Exception("La cantidad debe ser mayor a 0");

            var carrito = await _context.Carritos
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

            if (carrito == null)
                throw new Exception("No existe carrito para el usuario");

            var item = carrito.Items.FirstOrDefault(i => i.ProductoId == productoId);
            if (item == null)
                throw new Exception("El producto no existe en el carrito");

            item.Cantidad = cantidad;
            _context.CarritoItems.Update(item);
            await _context.SaveChangesAsync();

            return await ObtenerPorUsuarioIdAsync(usuarioId);
        }

        public async Task<bool> QuitarItemAsync(int usuarioId, int productoId)
        {
            var carrito = await _context.Carritos
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

            if (carrito == null)
                throw new Exception("No existe carrito para el usuario");

            var item = carrito.Items.FirstOrDefault(i => i.ProductoId == productoId);
            if (item == null)
                throw new Exception("El producto no existe en el carrito");

            _context.CarritoItems.Remove(item);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> VaciarAsync(int usuarioId)
        {
            var carrito = await _context.Carritos
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

            if (carrito == null)
                throw new Exception("No existe carrito para el usuario");

            _context.CarritoItems.RemoveRange(carrito.Items);
            await _context.SaveChangesAsync();
            return true;
        }

        private CarritoDTO Mapear(Carrito carrito)
        {
            return new CarritoDTO
            {
                Id = carrito.Id,
                UsuarioId = carrito.UsuarioId,
                Items = carrito.Items.Select(i => new CarritoItemDTO
                {
                    Id = i.Id,
                    ProductoId = i.ProductoId,
                    Cantidad = i.Cantidad,
                    ProductoNombre = i.Producto?.Nombre ?? string.Empty,
                    PrecioUnitario = i.Producto?.Precio ?? 0
                }).ToList()
            };
        }
    }
}
