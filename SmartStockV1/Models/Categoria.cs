namespace SmartStockV1.Models
{
    public class Categoria
    {
        public int CategoriaId { get; set; }
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = null!;
        public DateTime FechaCreacionCategoria { get; set; } = DateTime.UtcNow;
        public Usuario Usuario { get; set; } = null!;
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();

    }
}
