using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStockClienteWinFormsNet8.Dtos.Usuarios
{
    public class UsuarioResponseDto
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public int IdRol { get; set; }
        public string NombreRol { get; set; } = string.Empty;

        public bool EstaActivo { get; set; }
        public string EstadoDescripcion { get; set; } = string.Empty;

        public DateTime AltaUsuario { get; set; }
        public DateTime? HoraConexion { get; set; }
        public DateTime? UltConexion { get; set; }
        public DateTime UltActualizacion { get; set; }
    }
}