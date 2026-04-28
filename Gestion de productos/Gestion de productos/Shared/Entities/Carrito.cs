namespace Gestion_de_productos.Shared.Entities
{
    public class Carrito
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        // 🔗 Relación con Usuario
        public Usuario Usuario { get; set; }

        // 🔗 Relación con items del carrito (1 a muchos)
        public ICollection<CarritoItem> Items { get; set; } = new List<CarritoItem>();


    }
}
