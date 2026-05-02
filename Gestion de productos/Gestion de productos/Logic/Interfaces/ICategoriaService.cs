using Gestion_de_productos.Shared.DTOs;

public interface ICategoriaService
{
    Task<IEnumerable<CategoriaDTO>> ObtenerTodosAsync();

    Task<CategoriaDTO> ObtenerPorIdAsync(int id);

    Task<CategoriaDTO> CrearAsync(CrearCategoriaDTO dto);

    Task ActualizarAsync(int id, ActualizarCategoriaDTO dto);

    Task EliminarAsync(int id);
}