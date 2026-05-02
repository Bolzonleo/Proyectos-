using Gestion_de_productos.Shared.DTOs;

public interface IPedidoDetalleService
{
    Task<IEnumerable<PedidoDetalleDTO>> ObtenerPorPedidoIdAsync(int pedidoId);

    Task<PedidoDetalleDTO> AgregarDetalleAsync(int pedidoId, CrearPedidoDetalleDTO dto);

    Task EliminarDetalleAsync(int id);
}