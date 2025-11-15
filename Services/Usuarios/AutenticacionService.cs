using SmartStockV1.Models;
using SmartStockV1.Dtos.Usuarios;
using SmartStockV1.Interfaces.Usuarios;
using SmartStockV1.Repositories.Usuarios;
using System.Security.Cryptography;
using System.Text;

namespace SmartStockV1.Services.Usuarios
{
    public class AutenticacionService : IAutenticacionService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public AutenticacionService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        // ==================== LOGIN USUARIO ====================
        public async Task<UsuarioResponseDto> LoginUsuario(LoginUsuarioRequestDto dto)
        {
            var usuario = await _usuarioRepository.GetByNombreAsync(dto.NombreUsuario);
            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            if (usuario.EstadoUsuario == EstadoUsuario.Inactivo)
                throw new Exception("El usuario está inactivo.");

            if (usuario.ContraseñaHash == null || usuario.ContraseñaSalt == null)
                throw new Exception("El usuario no tiene contraseña configurada.");

            if (!VerificarPassword(dto.ContraseñaPlano, usuario.ContraseñaHash, usuario.ContraseñaSalt))
                throw new Exception("Contraseña incorrecta.");

            usuario.EstadoUsuario = EstadoUsuario.ActivoConectado;
            usuario.HoraConexion = DateTime.UtcNow;

            _usuarioRepository.Update(usuario);
            await _usuarioRepository.SaveChangesAsync();

            return MapToUsuarioResponseDto(usuario);
        }

        // ==================== LOGIN ADMIN ====================
        public async Task<AdminResponseDto> LoginAdmin(LoginAdminRequestDto dto)
        {
            var usuario = await _usuarioRepository.GetByNombreAsync(dto.NombreAdmin);
            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            if (usuario.RolId != (int)TipoRol.Admin)
                throw new Exception("El usuario no tiene rol de administrador.");

            if (usuario.EstadoUsuario == EstadoUsuario.Inactivo)
                throw new Exception("El administrador está inactivo.");

            if (usuario.ContraseñaHash == null || usuario.ContraseñaSalt == null)
                throw new Exception("El administrador no tiene contraseña configurada.");

            if (!VerificarPassword(dto.ContraseñaPlano, usuario.ContraseñaHash, usuario.ContraseñaSalt))
                throw new Exception("Contraseña incorrecta.");

            usuario.EstadoUsuario = EstadoUsuario.ActivoConectado;
            usuario.HoraConexion = DateTime.UtcNow;

            _usuarioRepository.Update(usuario);
            await _usuarioRepository.SaveChangesAsync();

            return MapToAdminResponseDto(usuario);
        }

        // ==================== LOGOUT USUARIO ====================
        public async Task<UsuarioResponseDto> LogoutUsuario(int idUsuario)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(idUsuario);
            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            usuario.EstadoUsuario = EstadoUsuario.ActivoDesconectado;
            usuario.UltConexion = DateTime.UtcNow;

            _usuarioRepository.Update(usuario);
            await _usuarioRepository.SaveChangesAsync();

            return MapToUsuarioResponseDto(usuario);
        }

        // ==================== HELPERS ====================

        private static bool VerificarPassword(string passwordPlano, byte[] hashAlmacenado, byte[] saltAlmacenada)
        {
            using var hmac = new HMACSHA512(saltAlmacenada);
            var hashComputado = hmac.ComputeHash(Encoding.UTF8.GetBytes(passwordPlano));
            return hashComputado.SequenceEqual(hashAlmacenado);
        }

        private static UsuarioResponseDto MapToUsuarioResponseDto(Usuario u)
        {
            bool estaActivo = u.EstadoUsuario != EstadoUsuario.Inactivo;

            string estadoDescripcion = u.EstadoUsuario switch
            {
                EstadoUsuario.Inactivo => "Inactivo",
                EstadoUsuario.ActivoDesconectado => "Activo (desconectado)",
                EstadoUsuario.ActivoConectado => "Activo (conectado)",
                _ => "Desconocido"
            };

            return new UsuarioResponseDto
            {
                IdUsuario = u.UsuarioId,
                NombreUsuario = u.UsuarioNombre,
                IdRol = u.RolId,
                NombreRol = u.Rol?.Nombre ?? ((TipoRol)u.RolId).ToString(),
                EstaActivo = estaActivo,
                EstadoDescripcion = estadoDescripcion,
                AltaUsuario = u.AltaUsuario,
                HoraConexion = u.HoraConexion,
                UltConexion = u.UltConexion,
                UltActualizacion = u.UltConexion ?? u.HoraConexion ?? u.AltaUsuario
            };
        }

        private static AdminResponseDto MapToAdminResponseDto(Usuario u)
        {
            bool estaActivo = u.EstadoUsuario != EstadoUsuario.Inactivo;

            string estadoDescripcion = u.EstadoUsuario switch
            {
                EstadoUsuario.Inactivo => "Inactivo",
                EstadoUsuario.ActivoDesconectado => "Activo (desconectado)",
                EstadoUsuario.ActivoConectado => "Activo (conectado)",
                _ => "Desconocido"
            };

            return new AdminResponseDto
            {
                IdUsuario = u.UsuarioId,
                NombreUsuario = u.UsuarioNombre,
                IdRol = u.RolId,
                NombreRol = u.Rol?.Nombre ?? ((TipoRol)u.RolId).ToString(),
                EstaActivo = estaActivo,
                EstadoDescripcion = estadoDescripcion,
                AltaUsuario = u.AltaUsuario,
                HoraConexion = u.HoraConexion,
                UltConexion = u.UltConexion,
                UltActualizacion = u.UltConexion ?? u.HoraConexion ?? u.AltaUsuario,
                EsAdmin = u.RolId == (int)TipoRol.Admin
            };
        }
    }
}