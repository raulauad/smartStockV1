using Microsoft.AspNetCore.Mvc;
using SmartStockV1.Dtos.Usuarios;
using SmartStockV1.Interfaces.Usuarios;

namespace SmartStockV1.Controllers
{
    /// <summary>
    /// Controlador para operaciones de administración de usuarios.
    /// En este caso, registro de administradores.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AdminUsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        // Id del rol ADMIN en tu tabla Rol (ajusta este valor según tu seed)
        private const int ADMIN_ROLE_ID = 1;

        public AdminUsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        /// <summary>
        /// Registra un nuevo usuario con rol ADMIN.
        /// CU01 - Alta de administrador.
        /// </summary>
        /// <remarks>
        /// - Requiere: NombreUsuario y ContraseñaPlano.
        /// - IdRol se fuerza al rol de Administrador en el servidor.
        /// </remarks>
        /// <param name="dto">Datos del nuevo admin (sin IdRol, se completa en el servidor).</param>
        /// <returns>Datos del admin creado.</returns>
        [HttpPost("registrar-admin")]
        // Más adelante, cuando tengas auth:
        // [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UsuarioResponseDto>> RegistrarAdmin([FromBody] AltaUsuarioRequestDto dto)
        {
            try
            {
                // Forzamos que este usuario sea ADMIN, sin depender de lo que mande el front.
                dto.IdRol = ADMIN_ROLE_ID;

                var creado = await _usuarioService.CrearUsuario(dto);

                // Devuelve 201 Created con la info del admin recién creado
                return CreatedAtAction(
                    nameof(ObtenerAdminPorId),
                    new { id = creado.IdUsuario },
                    creado
                );
            }
            catch (Exception ex)
            {
                // Para MVP está bien devolver BadRequest con el mensaje.
                // Después podés armar un middleware global de manejo de errores.
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un admin por Id (para comprobar el alta).
        /// </summary>
        [HttpGet("admin/{id:int}")]
        public async Task<ActionResult<UsuarioResponseDto>> ObtenerAdminPorId(int id)
        {
            try
            {
                var admin = await _usuarioService.ObtenerPorId(id);

                // Si querés asegurarte de que realmente sea admin, podrías verificar aquí el IdRol
                // if (admin.IdRol != ADMIN_ROLE_ID) return NotFound();

                return Ok(admin);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
