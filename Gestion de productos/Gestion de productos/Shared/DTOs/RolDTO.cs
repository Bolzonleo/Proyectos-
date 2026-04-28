namespace Gestion_de_productos.Shared.DTOs
{
    public class RolDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    public class CrearRolDTO
    {
        public string Nombre { get; set; } = string.Empty;
    }
}