namespace Gestion_de_productos.Models
{
    public class Producto
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public decimal Precio { get; set; }

        public int Stock { get; set; }

        // 🔗 Relación

        // 🔗 Clave Foránea
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        public ICollection<ImagenProducto> Imagenes { get; set; } = new List<ImagenProducto>();
    }
}
