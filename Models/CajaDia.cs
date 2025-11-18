namespace SmartStockV1.Models
{
    public class CajaDia
    {
        public int CajaDiaId { get; set; }
        public int DiaId { get; set; }
        public Dia Dia { get; set; } = null!;

        public decimal TotalCierre { get; set; }
        public decimal TotalGananciaDia { get; set; }
    }
}