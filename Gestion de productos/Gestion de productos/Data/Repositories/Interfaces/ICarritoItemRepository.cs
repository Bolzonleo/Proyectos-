using Gestion_de_productos.Shared.Entities;

public interface ICarritoItemRepository
{
    Task AgregarAsync(CarritoItem item);

    void Actualizar(CarritoItem item);

    void Eliminar(CarritoItem item);

    void EliminarRango(IEnumerable<CarritoItem> items);
}