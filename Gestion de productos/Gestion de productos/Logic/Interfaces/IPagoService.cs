using Gestion_de_productos.Shared.DTOs;

namespace Gestion_de_productos.Services.Interfaces
{
    public interface IPagoService
    {
        Task<IEnumerable<PagoDTO>> ObtenerTodosAsync();
        Task<PagoDTO> ObtenerPorIdAsync(int id);
        Task<PagoDTO> CrearAsync(CrearPagoDTO dto);
        Task<bool> ActualizarEstadoAsync(int id, ActualizarPagoDTO dto);
        Task<bool> EliminarAsync(int id);
    }
}
