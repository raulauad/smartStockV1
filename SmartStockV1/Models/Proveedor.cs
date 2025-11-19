namespace SmartStockV1.Models
{
    public class Proveedor
    {
        public int ProveedorId { get; set; }
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = null!;
        public string RazonSocial { get; set; }
        public string Cuit {  get; set; }
        public string DireccionProveedor { get; set; }
        public string TelefonoProveedor { get; set; } 
        public DateTime FechaCreacionProveedor { get; set; } = DateTime.UtcNow;
        public bool EstadoProveedor { get; set; }

        public Usuario Usuario { get; set; } = null!;
        public ICollection<DetalleCompra> DetallesCompra {  get; set; } = new List<DetalleCompra>();


    }
}
