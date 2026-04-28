using Gestion_de_productos.Shared.DTOs;

namespace Gestion_de_productos.Services.Interfaces
{
    public interface IProductoService
    {
        Task<IEnumerable<ProductoDTO>> ObtenerTodosAsync();
        Task<ProductoDTO> ObtenerPorIdAsync(int id);
        Task<IEnumerable<ProductoDTO>> ObtenerPorCategoriaAsync(int categoriaId);
        Task<IEnumerable<ProductoDTO>> BuscarPorNombreAsync(string termino);
        Task<ProductoDTO> CrearAsync(CrearProductoDTO dto);
        Task<bool> ActualizarAsync(int id, ActualizarProductoDTO dto);
        Task<bool> EliminarAsync(int id);
    }
}
