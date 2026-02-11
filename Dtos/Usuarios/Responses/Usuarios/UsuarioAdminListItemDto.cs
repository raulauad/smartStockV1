namespace SmartStockV1.Dtos.Usuarios.Responses.Usuarios
{

    public sealed record UsuarioAdminListItemDto(
        int IdUsuario,
        string NombreUsuario,
        string NombreRol,
        bool EstaActivo,
        DateTime FechaAlta,
        DateTime? UltimaConexion);
}
