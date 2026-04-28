using Gestion_de_productos.Shared.DTOs;

namespace Gestion_de_productos.Services.Interfaces
{
    public interface IPedidoDetalleService
    {
        Task<IEnumerable<PedidoDetalleDTO>> ObtenerPorPedidoIdAsync(int pedidoId);
        Task<PedidoDetalleDTO> AgregarDetalleAsync(int pedidoId, CrearPedidoDetalleDTO dto);
        Task<bool> EliminarDetalleAsync(int id);
    }
}
