using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartStockV1.Dtos.Usuarios.Requests.Auth;
using SmartStockV1.Dtos.Usuarios.Responses.Auth;
using SmartStockV1.Dtos.Usuarios.Responses.Usuarios;
using SmartStockV1.Interfaces.Usuarios;
using SmartStockV1.Models;
using SmartStockV1.Repositories.Usuarios;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SmartStockV1.Services.Usuarios
{
    public sealed class AutenticacionService : IAutenticacionService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _config;

        public AutenticacionService(IUsuarioRepository usuarioRepository, IConfiguration config)
        {
            _usuarioRepository = usuarioRepository;
            _config = config;
        }

        // Login único para usuario y admin
        public async Task<LoginResponseDto> Login(LoginUsuarioRequestDto dto)
        {
            if (dto is null) throw new ArgumentException("Body requerido.");

            var nombre = dto.NombreUsuario?.Trim();
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("NombreUsuario es requerido.");

            if (string.IsNullOrWhiteSpace(dto.ContraseñaPlano))
                throw new ArgumentException("ContraseñaPlano es requerida.");

            var usuario = await _usuarioRepository.GetByNombreAsync(nombre)
                ?? throw new KeyNotFoundException("Usuario no encontrado.");

            if (usuario.EstadoUsuario == EstadoUsuario.Inactivo)
                throw new InvalidOperationException("El usuario está inactivo.");

            if (usuario.ContraseñaHash is null || usuario.ContraseñaSalt is null)
                throw new InvalidOperationException("El usuario no tiene contraseña configurada.");

            if (!VerificarPassword(dto.ContraseñaPlano, usuario.ContraseñaHash, usuario.ContraseñaSalt))
                throw new UnauthorizedAccessException("Credenciales inválidas.");

            usuario.EstadoUsuario = EstadoUsuario.ActivoConectado;
            usuario.HoraConexion = DateTime.UtcNow;

            _usuarioRepository.Update(usuario);
            await _usuarioRepository.SaveChangesAsync();

            var usuarioDto = MapToUsuarioResponseDto(usuario);
            var token = GenerarJwt(usuarioDto);

            return new LoginResponseDto(token, usuarioDto);
        }

        // Logout de usuario/admin
        public async Task<UsuarioResponseDto> LogoutUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new ArgumentException("IdUsuario inválido.");

            var usuario = await _usuarioRepository.GetByIdAsync(idUsuario)
                ?? throw new KeyNotFoundException("Usuario no encontrado.");

            usuario.EstadoUsuario = EstadoUsuario.ActivoDesconectado;
            usuario.UltConexion = DateTime.UtcNow;

            _usuarioRepository.Update(usuario);
            await _usuarioRepository.SaveChangesAsync();

            return MapToUsuarioResponseDto(usuario);
        }

        // Generar JWT
        private string GenerarJwt(UsuarioResponseDto u)
        {
            var key = _config["Jwt:Key"] ?? throw new InvalidOperationException("Falta Jwt:Key.");
            var issuer = _config["Jwt:Issuer"] ?? "SmartStockV1";
            var audience = _config["Jwt:Audience"] ?? "SmartStockV1";
            var expiresMinutes = int.TryParse(_config["Jwt:ExpiresMinutes"], out var m) ? m : 30;

            var keyBytes = TryGetKeyBytes(key);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, u.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, u.NombreUsuario),
                new Claim(ClaimTypes.Role, u.NombreRol) // Para [Authorize(Roles="Admin")]
            };

            var signingKey = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private static byte[] TryGetKeyBytes(string key)
        {
            try
            {
                var bytes = Convert.FromBase64String(key);
                if (bytes.Length < 32)
                    throw new InvalidOperationException("Jwt:Key demasiado corta (mínimo 32 bytes/256 bits).");
                return bytes;
            }
            catch (FormatException)
            {
                var bytes = Encoding.UTF8.GetBytes(key);
                if (bytes.Length < 32)
                    throw new InvalidOperationException("Jwt:Key demasiado corta (mínimo 32 caracteres).");
                return bytes;
            }
        }

        // Verificar contraseña
        private static bool VerificarPassword(string passwordPlano, byte[] hashAlmacenado, byte[] saltAlmacenada)
        {
            using var hmac = new HMACSHA512(saltAlmacenada);
            var hashComputado = hmac.ComputeHash(Encoding.UTF8.GetBytes(passwordPlano));
            return hashComputado.SequenceEqual(hashAlmacenado);
        }

        // Mapper para respuesta de usuario
        private static UsuarioResponseDto MapToUsuarioResponseDto(Usuario u)
        {
            var estaActivo = u.EstadoUsuario != EstadoUsuario.Inactivo;
            var estadoDescripcion = u.EstadoUsuario switch
            {
                EstadoUsuario.Inactivo => "Inactivo",
                EstadoUsuario.ActivoDesconectado => "Activo (desconectado)",
                EstadoUsuario.ActivoConectado => "Activo (conectado)",
                _ => "Desconocido"
            };

            var nombreRol = u.Rol?.NombreRol ?? ((TipoRol)u.RolId).ToString();
            var ultActualizacion = u.UltConexion ?? u.HoraConexion ?? u.AltaUsuario;

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
    }
}