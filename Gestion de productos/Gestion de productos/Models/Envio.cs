namespace Gestion_de_productos.Models
{
    public class Envio
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public DateTime FechaEnvio { get; set; }
        public DateTime FechaEntrega { get; set; }
        public string Estado { get; set; } = string.Empty;
        // Relación con Pedido
        public Pedido Pedido { get; set; }
    }
}
