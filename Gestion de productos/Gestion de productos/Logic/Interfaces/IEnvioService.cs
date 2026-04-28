using Gestion_de_productos.Shared.DTOs;

namespace Gestion_de_productos.Services.Interfaces
{
    public interface IEnvioService
    {
        Task<IEnumerable<EnvioDTO>> ObtenerTodosAsync();
        Task<EnvioDTO> ObtenerPorIdAsync(int id);
        Task<EnvioDTO> CrearAsync(CrearEnvioDTO dto);
        Task<bool> ActualizarAsync(int id, ActualizarEnvioDTO dto);
        Task<bool> EliminarAsync(int id);
    }
}
