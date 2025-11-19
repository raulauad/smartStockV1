using Microsoft.AspNetCore.Identity;

namespace SmartStockV1.Dtos.Usuarios
{
    public class AltaUsuarioRequestDto
    {
        public string NombreUsuario { get; set; } = null!;
        public int IdRol { get; set; }
        public string ContraseñaPlano { get; set; } = null!;
    }

}
