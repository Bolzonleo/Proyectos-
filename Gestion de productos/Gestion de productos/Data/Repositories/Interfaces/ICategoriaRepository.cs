using Gestion_de_productos.Shared.Entities;

public interface ICategoriaRepository
{
    Task<IEnumerable<Categoria>> ObtenerTodosAsync();

    Task<Categoria?> ObtenerPorIdAsync(int id);

    Task<bool> ExistePorNombreAsync(string nombre, int? excluirId = null);

    Task CrearAsync(Categoria categoria);

    Task ActualizarAsync(Categoria categoria);

    Task EliminarAsync(Categoria categoria);

    Task<bool> TieneProductosAsync(int categoriaId);
}