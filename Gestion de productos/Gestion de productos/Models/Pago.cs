namespace Gestion_de_productos.Models
{
    public class Pago
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public string FormaPago { get; set; } = string.Empty;
        public DateTime FechaPago { get; set; }
        public string Estado { get; set; } = string.Empty;


        // Relación con Pedido
        public Pedido Pedido { get; set; }
    }
}
