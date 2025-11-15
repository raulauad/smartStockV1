namespace SmartStockV1.Models
{
    public class Dia
    {
        public int DiaId { get; set; }
        public DateTime FechaDia { get; set; } = DateTime.UtcNow;

        public ICollection<VentaDia> VentaDias { get; set; } = new List<VentaDia>();
        public ICollection<CompraDia> CompraDias { get; set; } = new List<CompraDia>();
        public ICollection<CajaDia> CajaDias { get; set; } = new List<CajaDia>();
    }
}
