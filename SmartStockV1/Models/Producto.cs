using System.Diagnostics.CodeAnalysis;

namespace SmartStockV1.Models
{
    public class Producto
    {
        public int ProductoId { get; set; }
        public int CategoriaId { get; set; }
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? CodigoBarra { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioCosto { get; set; }
        public decimal PrecioVenta { get; set; }
        public bool EstadoProducto { get; set; } = false;

        public Categoria Categoria { get; set; }
        public Usuario Usuario { get; set; }
        public StockActual? StockActual { get; set; }
        public ICollection<DetalleVentaItem> DetallesVentaItem { get; set; } = new List<DetalleVentaItem>();
        public ICollection<DetalleCompraItem> DetallesCompraItem { get; set; } = new List<DetalleCompraItem>();
        public ICollection<MovimientoStock> MovimientosStock { get; set; } = new List<MovimientoStock>();

    }
}
