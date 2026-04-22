using Gestion_de_productos.DTOs;

namespace Gestion_de_productos.Services.Interfaces
{
    public interface IRolService
    {
        Task<IEnumerable<RolDTO>> ObtenerTodosAsync();
        Task<RolDTO> ObtenerPorIdAsync(int id);
        Task<bool> ExistePorNombreAsync(string nombre, int? excluirId = null);
        Task<RolDTO> CrearAsync(CrearRolDTO dto);
        Task<bool> ActualizarAsync(int id, CrearRolDTO dto);
        Task<bool> EliminarAsync(int id);
    }
}
