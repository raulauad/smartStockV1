namespace SmartStockV1.Models
{
    public class Dia
    {
        public int DiaId { get; set; }
        public DateTime Fecha { get; set; }

        public CompraDia? CompraDia { get; set; }  // 1–1
        public VentaDia? VentaDia { get; set; }    // 1–1
        public CajaDia? CajaDia { get; set; }      // 1–1
    }
}
