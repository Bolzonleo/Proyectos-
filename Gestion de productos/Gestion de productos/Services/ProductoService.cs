using Gestion_de_productos.Data;
using Gestion_de_productos.DTOs;
using Gestion_de_productos.Models;
using Gestion_de_productos.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gestion_de_productos.Services
{
    public class ProductoService : IProductoService
    {
        private readonly AppDbContext _context;

        public ProductoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductoDTO>> ObtenerTodosAsync()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .ToListAsync();

            return productos.Select(p => new ProductoDTO
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Precio = p.Precio,
                Stock = p.Stock,
                CategoriaId = p.CategoriaId,
                CategoriaNombre = p.Categoria?.Nombre ?? string.Empty
            }).ToList();
        }

        public async Task<ProductoDTO> ObtenerPorIdAsync(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
                throw new Exception($"Producto con ID {id} no encontrado");

            return new ProductoDTO
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Stock = producto.Stock,
                CategoriaId = producto.CategoriaId,
                CategoriaNombre = producto.Categoria?.Nombre ?? string.Empty
            };
        }

        public async Task<IEnumerable<ProductoDTO>> ObtenerPorCategoriaAsync(int categoriaId)
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.CategoriaId == categoriaId)
                .ToListAsync();

            return productos.Select(p => new ProductoDTO
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Precio = p.Precio,
                Stock = p.Stock,
                CategoriaId = p.CategoriaId,
                CategoriaNombre = p.Categoria?.Nombre ?? string.Empty
            }).ToList();
        }

        public async Task<IEnumerable<ProductoDTO>> BuscarPorNombreAsync(string termino)
        {
            termino = termino.Trim();

            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.Nombre.Contains(termino))
                .ToListAsync();

            return productos.Select(p => new ProductoDTO
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Precio = p.Precio,
                Stock = p.Stock,
                CategoriaId = p.CategoriaId,
                CategoriaNombre = p.Categoria?.Nombre ?? string.Empty
            }).ToList();
        }

        public async Task<ProductoDTO> CrearAsync(CrearProductoDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new Exception("El nombre del producto es obligatorio");
            if (dto.Precio <= 0)
                throw new Exception("El precio debe ser mayor a 0");
            if (dto.Stock < 0)
                throw new Exception("El stock no puede ser negativo");

            // Verificar que la categoría existe
            var categoriaExiste = await _context.Categorias.AnyAsync(c => c.Id == dto.CategoriaId);
            if (!categoriaExiste)
                throw new Exception($"Categoría con ID {dto.CategoriaId} no encontrada");

            var producto = new Producto
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                Stock = dto.Stock,
                CategoriaId = dto.CategoriaId
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == dto.CategoriaId);

            return new ProductoDTO
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Stock = producto.Stock,
                CategoriaId = producto.CategoriaId,
                CategoriaNombre = categoria?.Nombre ?? string.Empty
            };
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarProductoDTO dto)
        {
            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.Id == id);
            if (producto == null)
                throw new Exception($"Producto con ID {id} no encontrado");

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new Exception("El nombre del producto es obligatorio");
            if (dto.Precio <= 0)
                throw new Exception("El precio debe ser mayor a 0");
            if (dto.Stock < 0)
                throw new Exception("El stock no puede ser negativo");

            producto.Nombre = dto.Nombre;
            producto.Descripcion = dto.Descripcion;
            producto.Precio = dto.Precio;
            producto.Stock = dto.Stock;

            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.Id == id);
            if (producto == null)
                throw new Exception($"Producto con ID {id} no encontrado");

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
