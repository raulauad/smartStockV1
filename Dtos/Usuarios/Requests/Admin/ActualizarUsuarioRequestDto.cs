namespace SmartStockV1.Dtos.Usuarios.Requests.Admin
{
    // Actualizar usuario 
    public sealed class ActualizarUsuarioRequestDto
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public int IdRol { get; set; }               // admin puede cambiar rol
        public bool EstadoUsuario { get; set; }      // activo / inactivo
        public string? DireccionUsuario { get; set; }
        public string? TelefonoUsuario { get; set; }
        public string? NuevaContraseñaPlano { get; set; }
    }

}
