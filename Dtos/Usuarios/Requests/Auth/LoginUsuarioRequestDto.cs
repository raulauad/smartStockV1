namespace SmartStockV1.Dtos.Usuarios.Requests.Auth
{
    public class LoginUsuarioRequestDto
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public string ContraseñaPlano { get; set; } = string.Empty;
    }
}
