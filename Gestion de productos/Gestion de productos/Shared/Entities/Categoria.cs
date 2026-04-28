namespace Gestion_de_productos.Shared.Entities
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        // 🔗 Relación con Producto (1 a muchos)
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
