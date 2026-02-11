using SmartStockV1.Models;

namespace SmartStockV1.Dtos.Usuarios.Responses.Usuarios
{
    public sealed record UsuarioResponseDto(
        int idUsuario,
        string NombreUsuario,
        string NombreRol,
        bool EstaActivo,
        string EstadoDescripcion,
        string? DireccionUsuario,
        string? TelefonoUsuario,
        DateTime AltaUsuario,
        DateTime? UltConexion,
        DateTime UltActualizacion); //Constructor de respuesta sellada inmutable
}

