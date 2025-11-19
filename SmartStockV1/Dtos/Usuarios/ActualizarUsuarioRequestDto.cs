namespace SmartStockV1.Dtos.Usuarios
{
    // Actualizar usuario (sólo admin)
    public class ActualizarUsuarioRequestDto
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public int IdRol { get; set; }               // admin puede cambiar rol
        public bool EstadoUsuario { get; set; }      // activo / inactivo
        public string? NuevaContraseñaPlano { get; set; }
    }

}
