using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStockClienteWinFormsNet8.Dtos.Usuarios
{
    public class ActualizarUsuarioRequestDto
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public int IdRol { get; set; }
        public bool EstadoUsuario { get; set; }
        public string NuevaContraseñaPlano { get; set; }
    }
}