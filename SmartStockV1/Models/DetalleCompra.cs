namespace SmartStockV1.Models
{
    public class DetalleCompra
    {
        public int DetalleCompraId { get; set; }
        public int CompraDiaId { get; set; }
        public int ProveedorId { get; set; }
        public int UsuarioId { get; set; }
        public decimal SubtotalDetalleCompra { get; set; } = 0;
        public DateTime FechaDetalleCompra { get; set; } = DateTime.UtcNow;

        public CompraDia CompraDia { get; set; } = null!;
        public Proveedor Proveedor { get; set; } = null!;
        public Usuario Usuario { get; set; } = null!;
        public ICollection<DetalleCompraItem> ItemsDetalleCompra { get; set; } = new List<DetalleCompraItem>();

    }
}
