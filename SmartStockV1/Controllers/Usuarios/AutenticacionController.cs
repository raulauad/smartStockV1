using Microsoft.AspNetCore.Mvc;
using SmartStockV1.Dtos.Usuarios;
using SmartStockV1.Interfaces.Usuarios;

namespace SmartStockV1.Controllers
{
    /// <summary>
    /// Controlador de autenticación:
    /// - Login de usuario (empleado o admin).
    /// - Login de administrador.
    /// - Logout de usuario.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AutenticacionController : ControllerBase
    {
        private readonly IAutenticacionService _autenticacionService;
    
        public AutenticacionController(IAutenticacionService autenticacionService)
        {
            _autenticacionService = autenticacionService;
        }
            
        /// <summary>       
        /// Login de USUARIO/EMPLEADO.  
        /// - Verifica nombre + contraseña. 
        /// - Verifica que NO esté inactivo.    
        /// - Deja al usuario en estado ActivoConectado.    
        /// </summary>  
        /// <param name="dto">NombreUsuario + ContraseñaPlano.</param>  
        [HttpPost("login-usuario")]     
        public async Task<ActionResult<UsuarioResponseDto>> LoginUsuario(   
            [FromBody] LoginUsuarioRequestDto dto)      
        {
            try
            {
                var usuario = await _autenticacionService.LoginUsuario(dto);
                return Ok(usuario);     
            }
            catch (Exception ex)    
            {
                // Para MVP: devolvemos BadRequest con mensaje.
                // Más adelante se puede reemplazar por middleware global de errores.
                return BadRequest(new { error = ex.Message });
            }   
        }   

        /// <summary>
        /// Login de ADMIN.
        /// - Verifica nombre + contraseña.
        /// - Verifica que Rol = Admin.
        /// - Deja al admin en estado ActivoConectado.
        /// </summary>
        /// <param name="dto">NombreUsuario + ContraseñaPlano.</param>
        [HttpPost("login-admin")]
        public async Task<ActionResult<AdminResponseDto>> LoginAdmin(
            [FromBody] LoginAdminRequestDto dto)
        {
            try
            {
                var admin = await _autenticacionService.LoginAdmin(dto);
                return Ok(admin);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Logout de un usuario (admin o empleado).
        /// - Cambia EstadoUsuario a ActivoDesconectado.
        /// - Setea UltConexion = DateTime.UtcNow.
        /// </summary>
        /// <param name="idUsuario">Id del usuario a desconectar.</param>
        [HttpPost("logout/{idUsuario:int}")]
        public async Task<ActionResult<UsuarioResponseDto>> LogoutUsuario(int idUsuario)
        {
            try
            {
                var usuario = await _autenticacionService.LogoutUsuario(idUsuario);
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
