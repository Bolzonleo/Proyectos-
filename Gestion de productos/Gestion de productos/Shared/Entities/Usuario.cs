namespace Gestion_de_productos.Shared.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Contraseña { get; set; } = string.Empty;
        public int RolId { get; set; }

        // 🔗 Relación con Rol
        public Rol Rol { get; set; }

        // 🔗 Relación con Pedido (1 a muchos)
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

        // 🔗 Relación con Carrito (1 a 1)
        public Carrito Carrito { get; set; }
    }
}
