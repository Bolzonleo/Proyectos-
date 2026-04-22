using Gestion_de_productos.DTOs;

namespace Gestion_de_productos.Services.Interfaces
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaDTO>> ObtenerTodosAsync();
        Task<CategoriaDTO> ObtenerPorIdAsync(int id);
        Task<bool> ExistePorNombreAsync(string nombre, int? excluirId = null);
        Task<CategoriaDTO> CrearAsync(CrearCategoriaDTO dto);
        Task<bool> ActualizarAsync(int id, ActualizarCategoriaDTO dto);
        Task<bool> EliminarAsync(int id);
    }
}
