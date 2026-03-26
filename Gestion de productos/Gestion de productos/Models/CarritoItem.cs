namespace Gestion_de_productos.Models
{
    public class CarritoItem
    {
        public int Id { get; set; }
        public int CarritoId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        // 🔗 Relación con Carrito
        public Carrito Carrito { get; set; }
        // 🔗 Relación con Producto
        public Producto Producto { get; set; }
    }
}
