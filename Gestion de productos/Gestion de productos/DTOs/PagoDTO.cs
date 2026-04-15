namespace Gestion_de_productos.DTOs
{
    public class PagoDTO
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public decimal Monto { get; set; }
        public string FormaPago { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaPago { get; set; }
    }

    public class CrearPagoDTO
    {
        public int PedidoId { get; set; }
        public decimal Monto { get; set; }
        public string FormaPago { get; set; } = string.Empty; // "Tarjeta", "Efectivo", etc.
    }

    public class ActualizarPagoDTO
    {
        public string Estado { get; set; } = string.Empty; // "Completado", "Fallido", etc.
    }
}