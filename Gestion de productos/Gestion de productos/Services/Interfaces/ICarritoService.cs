using Gestion_de_productos.DTOs;

namespace Gestion_de_productos.Services.Interfaces
{
    public interface ICarritoService
    {
        Task<CarritoDTO> ObtenerPorUsuarioIdAsync(int usuarioId);
        Task<CarritoDTO> CrearParaUsuarioAsync(int usuarioId);
        Task<CarritoDTO> AgregarItemAsync(int usuarioId, AgregarAlCarritoDTO dto);
        Task<CarritoDTO> ActualizarCantidadAsync(int usuarioId, int productoId, int cantidad);
        Task<bool> QuitarItemAsync(int usuarioId, int productoId);
        Task<bool> VaciarAsync(int usuarioId);
    }
}
