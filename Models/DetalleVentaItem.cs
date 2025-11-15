namespace SmartStockV1.Models
{
    public class DetalleVentaItem
    {
        public int VentaItemId { get; set; }
        public int DetalleVentaId { get; set; }
        public int ProductoId { get; set; }
        public decimal CantidadItem { get; set; } = 0;
        public decimal PrecioCostoItem { get; set; } = 0; //Costo al momento de la venta
        public decimal PrecioVentaItem { get; set; } = 0; //Precio de venta al momento de la venta

        //Navegacion
        public DetalleVenta DetalleVenta { get; set; } = null!;
        public Producto Producto { get; set; } = null!;
        public ICollection<MovimientoStock> MovimientosStock { get; set; } = new List<MovimientoStock>();

    }
}
