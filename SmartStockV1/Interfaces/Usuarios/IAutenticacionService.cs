namespace SmartStockV1.Interfaces.Usuarios
{
    using SmartStockV1.Dtos.Usuarios;
    /// Servicio para manejar el ingreso al sistema:
    /// - Login de empleado por nombre + contraseña
    /// - Login de admin con nombre + contraseña
    public interface IAutenticacionService
    {
        // Login de usuario/empleado:
        // - Verifica nombre + contraseña.
        // - Verifica que el usuario exista y NO esté inactivo.
        // - Setea EstadoUsuario = ActivoConectado.
        // - Setea HoraConexion = DateTime.UtcNow.
        // - Devuelve UsuarioResponseDto con el estado actualizado.
        Task<UsuarioResponseDto> LoginUsuario(LoginUsuarioRequestDto dto);

        // Login de admin:
        // - Verifica nombre + contraseña.
        // - Verifica Rol = Admin.
        // - Setea EstadoUsuario = ActivoConectado.
        // - Setea HoraConexion = DateTime.UtcNow.
        Task<AdminResponseDto> LoginAdmin(LoginAdminRequestDto dto);

        // - Setea EstadoUsuario = ActivoDesconectado.
        // - Setea UltConexion = DateTime.UtcNow.
        Task<UsuarioResponseDto> LogoutUsuario(int idUsuario);
    }
}
