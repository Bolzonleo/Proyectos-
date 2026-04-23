using Gestion_de_productos.DTOs;

namespace Gestion_de_productos.Services.Interfaces
{
    public interface IPedidoService
    {
        Task<IEnumerable<PedidoDTO>> ObtenerTodosAsync();
        Task<PedidoDTO> ObtenerPorIdAsync(int id);
        Task<PedidoDTO> CrearAsync(CrearPedidoDTO dto);
        Task<PedidoDTO> CrearDesdeCarritoAsync(int usuarioId);
        Task<bool> ActualizarEstadoAsync(int id, ActualizarPedidoDTO dto);
        Task<bool> EliminarAsync(int id);
    }
}
