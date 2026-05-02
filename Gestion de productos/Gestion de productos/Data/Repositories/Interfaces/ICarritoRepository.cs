using Gestion_de_productos.Shared.Entities;

public interface ICarritoRepository
{
    Task<Carrito?> ObtenerPorUsuarioIdAsync(int usuarioId);

    Task<Carrito?> ObtenerConItemsAsync(int usuarioId);

    Task CrearAsync(Carrito carrito);

    Task GuardarCambiosAsync();
}