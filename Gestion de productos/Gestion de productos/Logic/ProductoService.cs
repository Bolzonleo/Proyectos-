using Gestion_de_productos.Services.Interfaces;
using Gestion_de_productos.Shared.DTOs;
using Gestion_de_productos.Shared.Entities;


namespace Gestion_de_productos.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _repo;

        public ProductoService(
            IProductoRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ProductoDTO>>
            ObtenerTodosAsync()
        {
            var productos =
                await _repo.ObtenerTodosAsync();

            return productos
                .Select(MapearProducto)
                .ToList();
        }

        public async Task<ProductoDTO>
            ObtenerPorIdAsync(int id)
        {
            var producto =
                await _repo.ObtenerPorIdAsync(id);

            if (producto == null)
                throw new Exception(
                    $"Producto con ID {id} no encontrado");

            return MapearProducto(producto);
        }

        public async Task<IEnumerable<ProductoDTO>>
            ObtenerPorCategoriaAsync(
                int categoriaId)
        {
            var productos =
                await _repo.ObtenerPorCategoriaAsync(
                    categoriaId);

            return productos
                .Select(MapearProducto)
                .ToList();
        }

        public async Task<IEnumerable<ProductoDTO>>
            BuscarPorNombreAsync(
                string termino)
        {
            termino = termino.Trim();

            var productos =
                await _repo.BuscarPorNombreAsync(
                    termino);

            return productos
                .Select(MapearProducto)
                .ToList();
        }

        public async Task<ProductoDTO>
            CrearAsync(
                CrearProductoDTO dto)
        {
            ValidarProducto(dto);

            bool categoriaExiste =
                await _repo.ExisteCategoriaAsync(
                    dto.CategoriaId);

            if (!categoriaExiste)
                throw new Exception(
                    "Categoría no encontrada");

            var producto = new Producto
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                Stock = dto.Stock,
                CategoriaId = dto.CategoriaId
            };

            await _repo.CrearAsync(producto);

            var creado =
                await _repo.ObtenerPorIdAsync(
                    producto.Id);

            return MapearProducto(creado!);
        }

        public async Task<bool>
            ActualizarAsync(
                int id,
                ActualizarProductoDTO dto)
        {
            var producto =
                await _repo.ObtenerPorIdAsync(id);

            if (producto == null)
                throw new Exception(
                  $"Producto con ID {id} no encontrado");

            ValidarProducto(dto);

            producto.Nombre = dto.Nombre;
            producto.Descripcion = dto.Descripcion;
            producto.Precio = dto.Precio;
            producto.Stock = dto.Stock;

            await _repo.ActualizarAsync(producto);

            return true;
        }

        public async Task<bool>
            EliminarAsync(int id)
        {
            var producto =
                await _repo.ObtenerPorIdAsync(id);

            if (producto == null)
                throw new Exception(
                  $"Producto con ID {id} no encontrado");

            await _repo.EliminarAsync(producto);

            return true;
        }


        // ==========================
        // Métodos privados auxiliares
        // ==========================

        private ProductoDTO MapearProducto(
            Producto p)
        {
            return new ProductoDTO
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Precio = p.Precio,
                Stock = p.Stock,
                CategoriaId = p.CategoriaId,
                CategoriaNombre =
                    p.Categoria?.Nombre
                    ?? string.Empty
            };
        }


        private void ValidarProducto(
            CrearProductoDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new Exception(
                    "El nombre es obligatorio");

            if (dto.Precio <= 0)
                throw new Exception(
                    "Precio inválido");

            if (dto.Stock < 0)
                throw new Exception(
                    "Stock inválido");
        }


        private void ValidarProducto(
            ActualizarProductoDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new Exception(
                    "El nombre es obligatorio");

            if (dto.Precio <= 0)
                throw new Exception(
                    "Precio inválido");

            if (dto.Stock < 0)
                throw new Exception(
                    "Stock inválido");
        }

    }
}