namespace Gestion_de_productos.Shared.Entities
{
    public class PedidoDetalle
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public int PedidoId { get; set; }
        public int Cantidad { get; set; }
        public double PrecioUnitario { get; set; }
        
        // Relación con Pedido
        public Pedido Pedido { get; set; }
        
        // Relación con Producto
        public Producto Producto { get; set; }
    }
}
