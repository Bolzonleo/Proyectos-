using Gestion_de_productos.Data;
using Gestion_de_productos.DTOs;
using Gestion_de_productos.Models;
using Gestion_de_productos.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gestion_de_productos.Services
{
    public class ImagenProductoService : IImagenProductoService
    {
        private readonly AppDbContext _context;

        public ImagenProductoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ImagenProductoDTO>> ObtenerPorProductoIdAsync(int productoId)
        {
            var imagenes = await _context.ImagenesProducto
                .Where(i => i.ProductoId == productoId)
                .ToListAsync();

            return imagenes.Select(i => new ImagenProductoDTO
            {
                Id = i.Id,
                ProductoId = i.ProductoId,
                Url = i.Url
            }).ToList();
        }

        public async Task<ImagenProductoDTO> ObtenerPorIdAsync(int id)
        {
            var imagen = await _context.ImagenesProducto.FirstOrDefaultAsync(i => i.Id == id);
            if (imagen == null)
                throw new Exception("Imagen no encontrada");

            return new ImagenProductoDTO
            {
                Id = imagen.Id,
                ProductoId = imagen.ProductoId,
                Url = imagen.Url
            };
        }

        public async Task<ImagenProductoDTO> CrearAsync(CrearImagenProductoDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Url))
                throw new Exception("La URL es obligatoria");

            var productoExiste = await _context.Productos.AnyAsync(p => p.Id == dto.ProductoId);
            if (!productoExiste)
                throw new Exception("Producto no encontrado");

            var imagen = new ImagenProducto
            {
                ProductoId = dto.ProductoId,
                Url = dto.Url.Trim()
            };

            _context.ImagenesProducto.Add(imagen);
            await _context.SaveChangesAsync();

            return new ImagenProductoDTO
            {
                Id = imagen.Id,
                ProductoId = imagen.ProductoId,
                Url = imagen.Url
            };
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var imagen = await _context.ImagenesProducto.FirstOrDefaultAsync(i => i.Id == id);
            if (imagen == null)
                throw new Exception("Imagen no encontrada");

            _context.ImagenesProducto.Remove(imagen);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
