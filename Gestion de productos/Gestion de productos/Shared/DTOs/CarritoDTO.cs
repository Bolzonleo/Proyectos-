namespace Gestion_de_productos.Shared.DTOs
{
    public class CarritoDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public List<CarritoItemDTO> Items { get; set; } = new List<CarritoItemDTO>();
    }

    public class CarritoItemDTO
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public decimal PrecioUnitario { get; set; }
    }

    public class AgregarAlCarritoDTO
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
    }
}