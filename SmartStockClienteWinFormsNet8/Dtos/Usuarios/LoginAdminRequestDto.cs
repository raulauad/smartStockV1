using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStockClienteWinFormsNet8.Dtos.Usuarios
{
    public class LoginAdminRequestDto
    {
        public string NombreAdmin { get; set; } = string.Empty;
        public string ContraseñaPlano { get; set; } = string.Empty;
    }
}
