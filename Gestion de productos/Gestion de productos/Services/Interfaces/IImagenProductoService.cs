using Gestion_de_productos.DTOs;

namespace Gestion_de_productos.Services.Interfaces
{
    public interface IImagenProductoService
    {
        Task<IEnumerable<ImagenProductoDTO>> ObtenerPorProductoIdAsync(int productoId);
        Task<ImagenProductoDTO> ObtenerPorIdAsync(int id);
        Task<ImagenProductoDTO> CrearAsync(CrearImagenProductoDTO dto);
        Task<bool> EliminarAsync(int id);
    }
}
