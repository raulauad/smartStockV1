using SmartStockV1.Models;
using SmartStockV1.Interfaces.Usuarios;
using SmartStockV1.Repositories.Usuarios;
using System.Security.Cryptography;
using System.Text;
using SmartStockV1.Dtos.Usuarios.Requests.Admin;
using SmartStockV1.Dtos.Usuarios.Responses.Usuarios;

namespace SmartStockV1.Services.Usuarios
{
    // CU01 : Servicio de gestión de usuarios (alta, edición, consulta)
    // Lo usa el Admin desde el panel de administración
    public sealed class AdminUsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public AdminUsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        // ==================== HELPERS PRIVADOS ====================

        private static void CrearPasswordHash(string contraseña, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA512();
            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(contraseña));
        }

        private static void ValidarRol(int idRol)
        {
            if (!Enum.IsDefined(typeof(TipoRol), idRol))
                throw new ArgumentOutOfRangeException(nameof(idRol), $"El rol con Id {idRol} no es válido.");
        }

        private static string GetNombreRol(Usuario u)
            => u.Rol?.NombreRol ?? ((TipoRol)u.RolId).ToString();

        private static string GetEstadoDescripcion(EstadoUsuario estado)
            => estado switch
            {
                EstadoUsuario.Inactivo => "Inactivo",
                EstadoUsuario.ActivoDesconectado => "Activo (desconectado)",
                EstadoUsuario.ActivoConectado => "Activo (conectado)",
                _ => "Desconocido"
            };

        // Response (record): se construye, no se setea con object initializer si tu record es con ctor.
        private static UsuarioResponseDto MapToUsuarioResponseDto(Usuario u, DateTime ultActualizacion)
        {
            var nombreRol = GetNombreRol(u);
            var estaActivo = u.EstadoUsuario != EstadoUsuario.Inactivo;
            var estadoDescripcion = GetEstadoDescripcion(u.EstadoUsuario);

            // Si tu UsuarioResponseDto record es con constructor primario, usá esto:
            // return new UsuarioResponseDto(...);

            // Si tu record es "property record" con init, esto también sirve:
            return new UsuarioResponseDto(
                IdUsuario: u.UsuarioId,
                NombreUsuario: u.UsuarioNombre,
                IdRol: u.RolId,
                NombreRol: nombreRol,
                EstaActivo: estaActivo,
                EstadoDescripcion: estadoDescripcion,
                DireccionUsuario: u.UsuarioDireccion,
                TelefonoUsuario: u.UsuarioTelefono,
                AltaUsuario: u.AltaUsuario,
                HoraConexion: u.HoraConexion,
                UltConexion: u.UltConexion,
                UltActualizacion: ultActualizacion
            );
        }

        private static UsuarioAdminListItemDto MapToUsuarioAdminListItemDto(Usuario u)
        {
            var nombreRol = GetNombreRol(u);
            var estaActivo = u.EstadoUsuario != EstadoUsuario.Inactivo;

            return new UsuarioAdminListItemDto(
                IdUsuario: u.UsuarioId,
                NombreUsuario: u.UsuarioNombre,
                IdRol: u.RolId,
                NombreRol: nombreRol,
                EstaActivo: estaActivo,
                FechaAlta: u.AltaUsuario,
                UltimaConexion: u.UltConexion
            );
        }

        // ==================== CREAR USUARIO ====================

        public async Task<UsuarioResponseDto> CrearUsuario(AltaUsuarioRequestDto dto)
        {
            if (dto is null)
                throw new ArgumentException("El body es requerido.");

            var nombre = dto.NombreUsuario?.Trim();
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("NombreUsuario es requerido.");

            if (string.IsNullOrWhiteSpace(dto.ContraseñaPlano))
                throw new ArgumentException("La contraseña es obligatoria.");

            ValidarRol(dto.IdRol);

            var yaExiste = await _usuarioRepository.ExistsByNombreAsync(nombre);
            if (yaExiste)
                throw new InvalidOperationException("Ya existe un usuario con ese nombre.");

            CrearPasswordHash(dto.ContraseñaPlano, out var hash, out var salt);

            var nuevoUsuario = new Usuario
            {
                UsuarioNombre = nombre,
                UsuarioDireccion = dto.DireccionUsuario,
                UsuarioTelefono = dto.TelefonoUsuario,
                RolId = dto.IdRol,
                ContraseñaHash = hash,
                ContraseñaSalt = salt,
                AltaUsuario = DateTime.UtcNow,
                EstadoUsuario = EstadoUsuario.ActivoDesconectado
            };

            await _usuarioRepository.AddAsync(nuevoUsuario);
            await _usuarioRepository.SaveChangesAsync();

            // Releer para traer Rol si el repo lo incluye
            var creado = await _usuarioRepository.GetByIdAsync(nuevoUsuario.UsuarioId) ?? nuevoUsuario;

            return MapToUsuarioResponseDto(creado, DateTime.UtcNow);
        }

        // ==================== ACTUALIZAR USUARIO ====================

        public async Task<UsuarioResponseDto> ActualizarUsuario(ActualizarUsuarioRequestDto dto)
        {
            if (dto is null)
                throw new ArgumentException("El body es requerido.");

            if (dto.IdUsuario <= 0)
                throw new ArgumentException("IdUsuario inválido.");

            ValidarRol(dto.IdRol);

            var usuario = await _usuarioRepository.GetByIdAsync(dto.IdUsuario)
                ?? throw new KeyNotFoundException("Usuario no encontrado.");

            // Si permitís cambiar el nombre, validá duplicados
            var nuevoNombre = dto.NombreUsuario?.Trim();
            if (!string.IsNullOrWhiteSpace(nuevoNombre) && !string.Equals(nuevoNombre, usuario.UsuarioNombre, StringComparison.OrdinalIgnoreCase))
            {
                var existe = await _usuarioRepository.ExistsByNombreAsync(nuevoNombre);
                if (existe)
                    throw new InvalidOperationException("Ya existe un usuario con ese nombre.");

                usuario.UsuarioNombre = nuevoNombre;
            }
            else if (string.IsNullOrWhiteSpace(usuario.UsuarioNombre))
            {
                // Caso raro: debería no pasar por tu modelo
                throw new InvalidOperationException("El usuario no tiene NombreUsuario válido.");
            }

            usuario.RolId = dto.IdRol;
            usuario.UsuarioDireccion = dto.DireccionUsuario ?? usuario.UsuarioDireccion;
            usuario.UsuarioTelefono = dto.TelefonoUsuario;

            // bool -> enum
            if (dto.EstadoUsuario)
            {
                if (usuario.EstadoUsuario == EstadoUsuario.Inactivo)
                    usuario.EstadoUsuario = EstadoUsuario.ActivoDesconectado;
            }
            else
            {
                usuario.EstadoUsuario = EstadoUsuario.Inactivo;
            }

            if (!string.IsNullOrWhiteSpace(dto.NuevaContraseñaPlano))
            {
                CrearPasswordHash(dto.NuevaContraseñaPlano, out var hash, out var salt);
                usuario.ContraseñaHash = hash;
                usuario.ContraseñaSalt = salt;
            }

            _usuarioRepository.Update(usuario);
            await _usuarioRepository.SaveChangesAsync();

            var actualizado = await _usuarioRepository.GetByIdAsync(usuario.UsuarioId) ?? usuario;

            return MapToUsuarioResponseDto(actualizado, DateTime.UtcNow);
        }

        // ==================== CONSULTAS ====================

        public async Task<UsuarioResponseDto> ObtenerPorId(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new ArgumentException("IdUsuario inválido.");

            var usuario = await _usuarioRepository.GetByIdAsync(idUsuario)
                ?? throw new KeyNotFoundException("Usuario no encontrado.");

            var ultActividad = usuario.UltConexion ?? usuario.HoraConexion ?? usuario.AltaUsuario;
            return MapToUsuarioResponseDto(usuario, ultActividad);
        }

        public async Task<IEnumerable<UsuarioAdminListItemDto>> ListarTodos()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            return usuarios.Select(MapToUsuarioAdminListItemDto).ToList();
        }

        public async Task<IEnumerable<UsuarioAdminListItemDto>> ListarActivos()
        {
            var usuarios = await _usuarioRepository.GetActivoAsync();
            return usuarios.Select(MapToUsuarioAdminListItemDto).ToList();
        }

        // ==================== CAMBIOS DE ESTADO ====================

        public async Task DesactivarUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new ArgumentException("IdUsuario inválido.");

            var usuario = await _usuarioRepository.GetByIdAsync(idUsuario)
                ?? throw new KeyNotFoundException("Usuario no encontrado.");

            if (usuario.EstadoUsuario == EstadoUsuario.Inactivo)
                return; // idempotente

            usuario.EstadoUsuario = EstadoUsuario.Inactivo;

            _usuarioRepository.Update(usuario);
            await _usuarioRepository.SaveChangesAsync();
        }

        public async Task ActivarUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new ArgumentException("IdUsuario inválido.");

            var usuario = await _usuarioRepository.GetByIdAsync(idUsuario)
                ?? throw new KeyNotFoundException("Usuario no encontrado.");

            if (usuario.EstadoUsuario != EstadoUsuario.Inactivo)
                return; // idempotente

            usuario.EstadoUsuario = EstadoUsuario.ActivoDesconectado;

            _usuarioRepository.Update(usuario);
            await _usuarioRepository.SaveChangesAsync();
        }
    }
}


