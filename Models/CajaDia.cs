namespace SmartStockV1.Models
{
    public class CajaDia
    {
        public int CajaDiaId { get; set; }
        public int DiaId { get; set; }
        public int VentaDiaId { get; set; }
        public int CompraDiaId { get; set; }
        public decimal TotalCierre { get; set; } = 0; //Cuanto quedo en la caja al cerrar el dia
        public decimal TotalGananciaDia { get; set; } = 0; //Margen total de ganancia del dia (ventas - compras)
        public string? EstadoCierre { get; set; } // Abierta o Cerrada

        //Navegacion
        public Dia Dia { get; set; } = null!;
        public VentaDia VentaDia { get; set; } = null!;
        public CompraDia CompraDia { get; set; } = null!;
    }
}
