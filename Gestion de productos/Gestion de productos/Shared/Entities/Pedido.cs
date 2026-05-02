using Gestion_de_productos.Shared.Enums;

namespace Gestion_de_productos.Shared.Entities
{
    public class Pedido
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; }
        public double Total { get; set; }
        public EstadoPedido Estado { get; set; }
        //  Relación con Usuario
        public Usuario Usuario { get; set; }
        
        // Relación con Pago 
        public Pago Pago { get; set; }
        // Relación con Envío
        public Envio Envio { get; set; }

        //  Relación con PedidoDetalle 
        public ICollection<PedidoDetalle> Detalles { get; set; } = new List<PedidoDetalle>();
        
    }
}
