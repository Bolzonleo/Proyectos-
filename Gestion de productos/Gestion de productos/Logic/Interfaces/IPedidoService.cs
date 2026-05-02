using Gestion_de_productos.Shared.DTOs;
using Gestion_de_productos.Shared.Enums;

public interface IPedidoService
{
    Task<PedidoDTO> CrearDesdeCarritoAsync(int usuarioId);

    Task<PedidoDTO> ObtenerPorIdAsync(int id);
    Task CambiarEstadoAsync(int pedidoId, EstadoPedido nuevoEstado);
}