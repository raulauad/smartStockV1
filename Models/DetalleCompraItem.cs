namespace SmartStockV1.Models
{
    public class DetalleCompraItem
    {
        public int CompraItemId { get; set; }
        public int DetalleCompraId { get; set; }
        public int ProductoId { get; set; }
        public decimal CantidadItem { get; set; } 
        public decimal PrecioCostoItem { get; set; } = 0;

        public DetalleCompra DetalleCompra { get; set; } = null!;
        public Producto Producto { get; set; } = null!;
        public ICollection<MovimientoStock> MovimientosStock { get; set; } = new List<MovimientoStock>();

    }
}
