using Microsoft.AspNetCore.Identity;

namespace SmartStockV1.Dtos.Usuarios.Requests.Admin
{
    public sealed class AltaUsuarioRequestDto
    {
        public string NombreUsuario { get; set; } = null!;
        public int IdRol { get; set; } //El admin no elige el rol al crear admin, pero si al crear usuario normal
        public string ContraseñaPlano { get; set; } = null!;

        public string? DireccionUsuario { get; set; }
        public string? TelefonoUsuario { get; set; }
    }

}
