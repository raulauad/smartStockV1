using SmartStockV1.Dtos.Usuarios;

namespace SmartStockV1.Models
{

    public enum TipoRol
    {
        Admin = 1,
        Usuario = 2
    }
    public class Rol
    {
        public int RolId { get; set; }
        public string Nombre { get; set; }
        public ICollection<Usuario> UsuariosCreados { get; set; } = new List<Usuario>();
    }
}
