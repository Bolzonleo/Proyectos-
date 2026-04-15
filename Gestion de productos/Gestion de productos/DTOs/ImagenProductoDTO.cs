namespace Gestion_de_productos.DTOs
{
    public class ImagenProductoDTO
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public string Url { get; set; } = string.Empty;
    }

    public class CrearImagenProductoDTO
    {
        public int ProductoId { get; set; }
        public string Url { get; set; } = string.Empty;
    }
}