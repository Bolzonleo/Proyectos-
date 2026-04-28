namespace Gestion_de_productos.Shared.Entities
{
    public class ImagenProducto
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public string Url { get; set; } = string.Empty;

        // Relación con Producto
        public Producto Producto { get; set; }

    }
}
