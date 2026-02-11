using SmartStockV1.Dtos.Usuarios.Responses.Usuarios;

namespace SmartStockV1.Dtos.Usuarios.Responses.Auth
{
    public sealed record LoginResponseDto(
        string Token,
        UsuarioResponseDto Usuario);
}
