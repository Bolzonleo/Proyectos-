namespace Gestion_de_productos.Shared.DTOs
{
    public class CategoriaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    public class CrearCategoriaDTO
    {
        public string Nombre { get; set; } = string.Empty;
    }

    public class ActualizarCategoriaDTO
    {
        public string Nombre { get; set; } = string.Empty;
    }
}