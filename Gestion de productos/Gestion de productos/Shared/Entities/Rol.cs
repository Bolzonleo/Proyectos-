namespace Gestion_de_productos.Shared.Entities
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        // 🔗 Relación con Usuario (1 a muchos)
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
