using Gestion_de_productos.Shared.Entities;

public interface IPedidoRepository
{
    Task CrearAsync(Pedido pedido);

    Task<Pedido?> ObtenerPorIdAsync(int id);

    Task GuardarCambiosAsync();
}