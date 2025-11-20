using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStockClienteWinFormsNet8.Dtos.Usuarios
{
    public class AltaAdminRequestDto
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public string ContraseñaPlano { get; set; } = string.Empty;
    }
}