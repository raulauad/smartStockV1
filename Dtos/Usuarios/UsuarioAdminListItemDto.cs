namespace SmartStockV1.Dtos.Usuarios
{
    // Versión liviana para listado
    public class UsuarioAdminListItemDto
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;

        public int IdRol { get; set; }
        public string NombreRol { get; set; } = string.Empty;

        public bool EstaActivo { get; set; }      // true = no inactivo
        public DateTime FechaAlta { get; set; }
        public DateTime? UltimaConexion { get; set; }
    }
}
