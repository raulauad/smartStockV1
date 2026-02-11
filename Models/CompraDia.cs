namespace SmartStockV1.Models
{
    public class CompraDia
    {
        public int CompraDiaId { get; set; }
        public int DiaId { get; set; }
        public Dia Dia { get; set; } = null!;

        public decimal TotalCompraDia { get; set; }
        public ICollection<DetalleCompra> DetallesCompra { get; set; } = new List<DetalleCompra>();
    }
}
