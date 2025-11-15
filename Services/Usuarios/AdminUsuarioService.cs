using SmartStockV1.Models;
using SmartStockV1.Dtos.Usuarios;
using SmartStockV1.Interfaces.Usuarios;
using SmartStockV1.Repositories.Usuarios;
using System.Security.Cryptography;
using System.Text;

namespace SmartStockV1.Services.Usuarios
{
    // CU01 : Servicio de gestión de usuarios (alta, edición, consulta)
    // Lo usa el Admin desde el panel de administración
    public class AdminUsuarioService : IUsuarioService
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

        /// <summary>
        /// Valida que el IdRol que viene en el DTO corresponda a un valor del enum TipoRol.
        /// </summary>
        private static void ValidarRol(int idRol)
        {
            if (!Enum.IsDefined(typeof(TipoRol), idRol))
                throw new Exception($"El rol con Id {idRol} no es un rol válido.");
        }

        /// <summary>
        /// Mapea la entidad Usuario al DTO que usa el Admin.
        /// </summary>
        private static UsuarioResponseDto MapToUsuarioResponseDto(Usuario u, DateTime ultActualizacion)
        {
            // Por si la navegación Rol viene null, usamos el enum como respaldo
            string nombreRol = u.Rol?.Nombre
                               ?? ((TipoRol)u.RolId).ToString();

            // bool EstadoUsuario = true si NO está Inactivo
            bool estaActivo = u.EstadoUsuario != EstadoUsuario.Inactivo;

            // Descripción legible del estado
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
                NombreRol = nombreRol,
                EstaActivo = estaActivo,
                EstadoDescripcion = estadoDescripcion,
                AltaUsuario = u.AltaUsuario,
                HoraConexion = u.HoraConexion,
                UltConexion = u.UltConexion,
                UltActualizacion = ultActualizacion
            };
        }

        /// <summary> Listar usuarios </summary>
        private static UsuarioAdminListItemDto MapToUsuarioAdminListItemDto(Usuario u)
        {
            return new UsuarioAdminListItemDto
            {
                IdUsuario = u.UsuarioId,
                NombreUsuario = u.UsuarioNombre,
                IdRol = u.RolId,
                NombreRol = u.Rol?.Nombre ?? ((TipoRol)u.RolId).ToString(),
                EstaActivo = u.EstadoUsuario != EstadoUsuario.Inactivo,
                FechaAlta = u.AltaUsuario,
                UltimaConexion = u.UltConexion
            };
        }

        // ==================== CREAR USUARIO (ADMIN / USUARIO) ====================

        public async Task<UsuarioResponseDto> CrearUsuario(AltaUsuarioRequestDto dto)
        {
            // 1) Validar duplicado
            var yaExiste = await _usuarioRepository.ExistsByNombreAsync(dto.NombreUsuario);
            if (yaExiste)
                throw new Exception("Ya existe un usuario con ese nombre.");

            // 2) Validar rol contra el enum
            ValidarRol(dto.IdRol);

            // 3) Validar contraseña
            if (string.IsNullOrWhiteSpace(dto.ContraseñaPlano))
                throw new Exception("La contraseña es obligatoria.");

            // 4) Generar hash
            CrearPasswordHash(dto.ContraseñaPlano, out var hash, out var salt);

            // 5) Construir entidad de dominio
            var nuevoUsuario = new Usuario
            {
                UsuarioNombre = dto.NombreUsuario,
                RolId = dto.IdRol, // puede ser Admin o Usuario, según lo haya decidido el Admin
                ContraseñaHash = hash,
                ContraseñaSalt = salt,
                AltaUsuario = DateTime.UtcNow,
                EstadoUsuario = EstadoUsuario.ActivoDesconectado
            };

            // 6) Persistir
            await _usuarioRepository.AddAsync(nuevoUsuario);
            await _usuarioRepository.SaveChangesAsync();

            // 7) Releer con Rol incluido
            var creado = await _usuarioRepository.GetByIdAsync(nuevoUsuario.UsuarioId) ?? nuevoUsuario;

            return MapToUsuarioResponseDto(creado, DateTime.UtcNow);
        }

        // ==================== ACTUALIZAR USUARIO ====================

        public async Task<UsuarioResponseDto> ActualizarUsuario(ActualizarUsuarioRequestDto dto)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(dto.IdUsuario);
            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            // Validar que el rol que quiere asignar el Admin exista
            ValidarRol(dto.IdRol);

            // Datos básicos
            usuario.UsuarioNombre = dto.NombreUsuario;
            usuario.RolId = dto.IdRol;

            // Mapear bool -> enum EstadoUsuario
            if (dto.EstadoUsuario)
            {
                if (usuario.EstadoUsuario == EstadoUsuario.Inactivo)
                    usuario.EstadoUsuario = EstadoUsuario.ActivoDesconectado;
            }
            else
            {
                usuario.EstadoUsuario = EstadoUsuario.Inactivo;
            }

            // Cambio de contraseña (solo si viene una nueva)
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
            var usuario = await _usuarioRepository.GetByIdAsync(idUsuario);
            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            var ultAct = usuario.UltConexion ?? usuario.AltaUsuario;

            return MapToUsuarioResponseDto(usuario, ultAct);
        }


        // Listar todos los usuarios (admin)
        public async Task<IEnumerable<UsuarioAdminListItemDto>> ListarTodos()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();

            return usuarios
                .Select(u => MapToUsuarioAdminListItemDto(u))
                .ToList();
        }

        // Listar solo usuarios activos (admin)
        public async Task<IEnumerable<UsuarioAdminListItemDto>> ListarActivos()
        {
            var usuarios = await _usuarioRepository.GetActivoAsync();

            return usuarios
                .Select(u => MapToUsuarioAdminListItemDto(u))
                .ToList();
        }

        // ==================== CAMBIOS DE ESTADO ====================

        public async Task DesactivarUsuario(int idUsuario)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(idUsuario);
            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            usuario.EstadoUsuario = EstadoUsuario.Inactivo;

            _usuarioRepository.Update(usuario);
            await _usuarioRepository.SaveChangesAsync();
        }

        public async Task ActivarUsuario(int idUsuario)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(idUsuario);
            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            usuario.EstadoUsuario = EstadoUsuario.ActivoDesconectado;

            _usuarioRepository.Update(usuario);
            await _usuarioRepository.SaveChangesAsync();
        }
    }
}

