using Gestion_de_productos.Services.Interfaces;
using Gestion_de_productos.Shared.DTOs;
using Gestion_de_productos.Shared.Entities;
using Gestion_de_productos.Data.Repositories;

namespace Gestion_de_productos.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _repo;

        public CategoriaService(ICategoriaRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<CategoriaDTO>> ObtenerTodosAsync()
        {
            var categorias = await _repo.ObtenerTodosAsync();

            return categorias.Select(Mapear);
        }

        public async Task<CategoriaDTO> ObtenerPorIdAsync(int id)
        {
            var categoria = await _repo.ObtenerPorIdAsync(id);

            if (categoria == null)
                throw new Exception($"Categoría con ID {id} no encontrada");

            return Mapear(categoria);
        }

        public async Task<CategoriaDTO> CrearAsync(CrearCategoriaDTO dto)
        {
            Validar(dto.Nombre);

            var existe = await _repo.ExistePorNombreAsync(dto.Nombre);

            if (existe)
                throw new Exception("Ya existe una categoría con ese nombre");

            var categoria = new Categoria
            {
                Nombre = dto.Nombre.Trim()
            };

            await _repo.CrearAsync(categoria);

            return Mapear(categoria);
        }

        public async Task ActualizarAsync(int id, ActualizarCategoriaDTO dto)
        {
            var categoria = await _repo.ObtenerPorIdAsync(id);

            if (categoria == null)
                throw new Exception($"Categoría con ID {id} no encontrada");

            Validar(dto.Nombre);

            var existe = await _repo.ExistePorNombreAsync(dto.Nombre, id);

            if (existe)
                throw new Exception("Ya existe una categoría con ese nombre");

            categoria.Nombre = dto.Nombre.Trim();

            await _repo.ActualizarAsync(categoria);
        }

        public async Task EliminarAsync(int id)
        {
            var categoria = await _repo.ObtenerPorIdAsync(id);

            if (categoria == null)
                throw new Exception($"Categoría con ID {id} no encontrada");

            var tieneProductos = await _repo.TieneProductosAsync(id);

            if (tieneProductos)
                throw new Exception("No se puede eliminar la categoría porque tiene productos asociados");

            await _repo.EliminarAsync(categoria);
        }


        // ==========================
        // Métodos privados
        // ==========================

        private CategoriaDTO Mapear(Categoria c)
        {
            return new CategoriaDTO
            {
                Id = c.Id,
                Nombre = c.Nombre
            };
        }

        private void Validar(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception("El nombre de la categoría es obligatorio");
        }
    }
}