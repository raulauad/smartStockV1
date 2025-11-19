using SmartStockV1.Models;

namespace SmartStockV1.Dtos.Usuarios
{
    public class AdminResponseDto
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;

        public int IdRol { get; set; }
        public bool EsAdmin { get; set; }
        public string NombreRol { get; set; } = string.Empty;


        public bool EstaActivo { get; set; } // true = no inactivo
        public string EstadoDescripcion { get; set; } = string.Empty;

        public DateTime AltaUsuario { get; set; }
        public DateTime? HoraConexion { get; set; }
        public DateTime? UltConexion { get; set; }
        public DateTime UltActualizacion { get; set; }

    }
}

