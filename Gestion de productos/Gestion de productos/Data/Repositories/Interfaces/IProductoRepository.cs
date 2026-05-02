using Gestion_de_productos.Shared.Entities;

public interface IProductoRepository
{
    Task<List<Producto>> ObtenerTodosAsync();

    Task<Producto?> ObtenerPorIdAsync(int id);

    Task<List<Producto>> ObtenerPorCategoriaAsync(int categoriaId);

    Task<List<Producto>> BuscarPorNombreAsync(string termino);

    Task CrearAsync(Producto producto);

    Task ActualizarAsync(Producto producto);

    Task EliminarAsync(Producto producto);

    Task<bool> ExisteCategoriaAsync(int categoriaId);
}