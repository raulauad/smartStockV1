namespace SmartStockV1.Models
{
    public class CompraDia
    {
        public int CompraDiaId { get; set; }
        public int DiaId { get; set; } = 0;
        public decimal TotalCompraDia { get; set; } = 0; //Acumula subtotales detalleCompra

        //Navegacion
        public Dia Dia { get; set; } = null!;
        public ICollection<DetalleCompra> DetallesCompra { get; set; } = new List<DetalleCompra>();
        public CajaDia? CajaDia { get; set; }
    }
}
