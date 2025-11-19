namespace SmartStockV1.Models
{
    public class DetalleVenta
    {
        public int DetalleVentaId { get; set; }
        public int VentaDiaId { get; set; }
        public int UsuarioId { get; set; }
        public decimal SubtotalDetalleVenta { get; set; }
        public DateTime FechDetalleVenta { get; set; } = DateTime.UtcNow;

        public VentaDia VentaDia { get; set; } = null!;
        public Usuario Usuario { get; set; }
        public ICollection<DetalleVentaItem> ItemsDetalleVenta { get; set; } = new List<DetalleVentaItem>();
        
    }
}
