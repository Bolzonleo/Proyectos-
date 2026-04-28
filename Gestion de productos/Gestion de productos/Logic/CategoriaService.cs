using Gestion_de_productos.Data.Context;
using Gestion_de_productos.Models;
using Gestion_de_productos.Services.Interfaces;
using Gestion_de_productos.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Gestion_de_productos.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly AppDbContext _context;

        public CategoriaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoriaDTO>> ObtenerTodosAsync()
        {
            var categorias = await _context.Categorias.ToListAsync();
            return categorias.Select(c => new CategoriaDTO
            {
                Id = c.Id,
                Nombre = c.Nombre
            }).ToList();
        }

        public async Task<CategoriaDTO> ObtenerPorIdAsync(int id)
        {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);
            if (categoria == null)
                throw new Exception($"Categoría con ID {id} no encontrada");

            return new CategoriaDTO
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre
            };
        }

        public async Task<bool> ExistePorNombreAsync(string nombre, int? excluirId = null)
        {
            var nombreNormalizado = nombre.Trim().ToLower();

            return await _context.Categorias
                .AnyAsync(c => c.Nombre.ToLower() == nombreNormalizado && (!excluirId.HasValue || c.Id != excluirId.Value));
        }

        public async Task<CategoriaDTO> CrearAsync(CrearCategoriaDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new Exception("El nombre de la categoría es obligatorio");

            var existe = await ExistePorNombreAsync(dto.Nombre);
            if (existe)
                throw new Exception("Ya existe una categoría con ese nombre");

            var categoria = new Categoria
            {
                Nombre = dto.Nombre
            };

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            return new CategoriaDTO
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre
            };
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarCategoriaDTO dto)
        {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);
            if (categoria == null)
                throw new Exception($"Categoría con ID {id} no encontrada");

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new Exception("El nombre de la categoría es obligatorio");

            var existe = await ExistePorNombreAsync(dto.Nombre, id);
            if (existe)
                throw new Exception("Ya existe una categoría con ese nombre");

            categoria.Nombre = dto.Nombre;
            _context.Categorias.Update(categoria);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);
            if (categoria == null)
                throw new Exception($"Categoría con ID {id} no encontrada");

            var tieneProductosAsociados = await _context.Productos.AnyAsync(p => p.CategoriaId == id);
            if (tieneProductosAsociados)
                throw new Exception("No se puede eliminar la categoría porque tiene productos asociados");

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
