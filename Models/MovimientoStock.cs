namespace SmartStockV1.Models
{
    public class MovimientoStock
    {
        public int MovimientoId{ get; set; }
        public int ProductoId { get; set; }
        public string Tipo { get; set; } //compra = ingreso, venta = egreso o ajuste
        public int? CompraItemId { get; set; } // referencia a DetalleCompraItem
        public int? VentaItemId { get; set; } // referencia a DetalleVentaItem
        public decimal Cantidad { get; set; }
        public string? MotivoAjuste { get; set; } // opcional, solo para ajustes
        public DateTime FechaMovimiento { get; set; } = DateTime.UtcNow; // fecha y hora del movimiento

        public Producto Producto { get; set; } = null!;
        public DetalleCompraItem? CompraItem { get; set; }
        public DetalleVentaItem? VentaItem { get; set; }
    }
}
