namespace Gestion_de_productos.DTOs
{
    public class PedidoDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; }
        public double Total { get; set; }
        public string Estado { get; set; } = string.Empty;
    }

    public class CrearPedidoDTO
    {
        public int UsuarioId { get; set; }
        public string Estado { get; set; } = "Pendiente";
    }

    public class ActualizarPedidoDTO
    {
        public string Estado { get; set; } = string.Empty;
    }
}