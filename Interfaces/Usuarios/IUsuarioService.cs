using SmartStockV1.Dtos.Usuarios;
using System.Collections.Generic;

namespace SmartStockV1.Interfaces.Usuarios
{
    /// Servicio para CU01: Registro y gestión de usuarios (alta, edición, consulta).
    /// Lo usa el Admin desde el panel de administración.
    public interface IUsuarioService
    {
        Task<UsuarioResponseDto> CrearUsuario(AltaUsuarioRequestDto dto); //Da de alta un nuevo usuario
        Task<UsuarioResponseDto> ActualizarUsuario(ActualizarUsuarioRequestDto dto); //Actualiza los datos de un usuario existente
        Task<UsuarioResponseDto> ObtenerPorId(int idUsuario); //Obtiene los datos de un usuario por su ID
        Task<IEnumerable<UsuarioAdminListItemDto>> ListarTodos(); //Lista todos los usuarios, tanto activos como inactivos
        Task<IEnumerable<UsuarioAdminListItemDto>> ListarActivos(); //Lista solo los usuarios activos
        Task DesactivarUsuario(int idUsuario); //Desactiva un usuario (cambia su estado a inactivo)
        Task ActivarUsuario(int idUsuario); //Activa un usuario (cambia su estado a activo)

    }
}
