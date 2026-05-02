using Gestion_de_productos.Shared.Entities;

public interface IPedidoDetalleRepository
{
    Task<IEnumerable<PedidoDetalle>> ObtenerPorPedidoIdAsync(int pedidoId);

    Task<PedidoDetalle?> ObtenerPorIdAsync(int id);

    Task AgregarAsync(PedidoDetalle detalle);

    void Eliminar(PedidoDetalle detalle);
    Task AgregarRangoAsync(IEnumerable<PedidoDetalle> detalles);

}