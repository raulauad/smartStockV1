namespace SmartStockV1.Models
{
    public class StockActual
    {
        public int ProductoId { get; set; }
        public int CantidadStock { get; set; } = 0;
        public DateTime UltActualizacion { get; set; } = DateTime.UtcNow;

        public Producto Producto { get; set; } = null!;

    }
}
