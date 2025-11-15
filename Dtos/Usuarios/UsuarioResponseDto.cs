namespace SmartStockV1.Dtos.Usuarios
{
    public class UsuarioResponseDto
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;

        public int IdRol { get; set; }
        public string NombreRol { get; set; } = string.Empty;

        public bool EstaActivo { get; set; }// Mapea al enum EstadoUsuario (0 = Inactivo, 1 = ActivoDesconectado, 2 = ActivoConectado), valor numérico del enum
        public string EstadoDescripcion { get; set; } = string.Empty;// "Inactivo", "Activo/Conectado", etc.
        public DateTime AltaUsuario { get; set; }   // Momento en que se dio de alta
        public DateTime? HoraConexion { get; set; } // Último momento que se conectó
        public DateTime? UltConexion { get; set; }  //  momento que se desconectó
        public DateTime UltActualizacion { get; set; }  // Última actualización del registro de usuario
    }
}

