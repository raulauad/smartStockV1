using Microsoft.AspNetCore.Mvc;
using SmartStockV1.Dtos.Usuarios;
using SmartStockV1.Interfaces.Usuarios;

namespace SmartStockV1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminUsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        private const int ADMIN_ROLE_ID = 1; // TipoRol.Admin

        public AdminUsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("registrar-admin")]
        public async Task<ActionResult<UsuarioResponseDto>> RegistrarAdmin([FromBody] AltaAdminRequestDto dto)
        {
            try
            {
                var altaInterna = new AltaUsuarioRequestDto
                {
                    NombreUsuario = dto.NombreUsuario,
                    ContraseñaPlano = dto.ContraseñaPlano,
                    IdRol = ADMIN_ROLE_ID   // se fuerza en el servidor
                };

                var creado = await _usuarioService.CrearUsuario(altaInterna);

                return CreatedAtAction(
                    nameof(ObtenerAdminPorId),
                    new { id = creado.IdUsuario },
                    creado
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("admin/{id:int}")]
        public async Task<ActionResult<UsuarioResponseDto>> ObtenerAdminPorId(int id)
        {
            try
            {
                var admin = await _usuarioService.ObtenerPorId(id);
                return Ok(admin);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
