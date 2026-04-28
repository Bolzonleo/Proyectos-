using Gestion_de_productos.Shared.DTOs;

namespace Gestion_de_productos.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioDTO>> ObtenerTodosAsync();
        Task<UsuarioDTO> ObtenerPorIdAsync(int id);
        Task<UsuarioDTO> CrearAsync(CrearUsuarioDTO dto);
        Task<bool> ActualizarAsync(int id, ActualizarUsuarioDTO dto);
        Task<bool> EliminarAsync(int id);
    }
}
