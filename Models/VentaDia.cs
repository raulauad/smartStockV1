namespace SmartStockV1.Models
{
    public class VentaDia
    {
        public int VentaDiaId { get; set; }
        public int DiaId { get; set; }
        public Dia Dia { get; set; } = null!;

        public decimal TotalVentaDia { get; set; }
        public decimal TotalGananciaVenta { get; set; }
        public ICollection<DetalleVenta> DetallesVentas { get; set; } = new List<DetalleVenta>();
    }
}
