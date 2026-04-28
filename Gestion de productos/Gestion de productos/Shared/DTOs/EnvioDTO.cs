namespace Gestion_de_productos.Shared.DTOs
{
    public class EnvioDTO
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaEnvio { get; set; }
        public DateTime? FechaEntrega { get; set; }
    }

    public class CrearEnvioDTO
    {
        public int PedidoId { get; set; }
        public string Direccion { get; set; } = string.Empty;
    }

    public class ActualizarEnvioDTO
    {
        public string Estado { get; set; } = string.Empty;
        public DateTime? FechaEntrega { get; set; }
    }
}