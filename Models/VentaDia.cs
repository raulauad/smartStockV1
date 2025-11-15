namespace SmartStockV1.Models
{
    public class VentaDia
    {
        public int VentaDiaId { get; set; }
        public int IdDia { get; set; }
        public decimal TotalVentaDia { get; set; } = 0; //(VentaDia - CompraDia)
        public decimal TotalGananciaVenta { get; set; } = 0;

        public Dia Dia { get; set; } = null!;
        public ICollection<DetalleVenta> DetallesVentas { get; set; } = new List<DetalleVenta>();
        public CajaDia? CajaDia { get; set; } 

    }
}
