using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStockV1.Dtos.Usuarios.Requests.Auth;
using SmartStockV1.Dtos.Usuarios.Responses.Auth;
using SmartStockV1.Dtos.Usuarios.Responses.Usuarios;
using SmartStockV1.Interfaces.Usuarios;
using System.Security.Claims;

namespace SmartStockV1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutenticacionController : ControllerBase
    {
        private readonly IAutenticacionService _autenticacionService;

        public AutenticacionController(IAutenticacionService autenticacionService)
        {
            _autenticacionService = autenticacionService;
        }


        // Login (usuario o admin).
        // Devuelve JWT + datos del usuario.
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginUsuarioRequestDto dto)
        {
            // Sin try/catch: el middleware global convierte exceptions a HTTP
            var result = await _autenticacionService.Login(dto);
            return Ok(result);
        }

        // Logout seguro: toma el Id del token.
        // Mantiene tu tracking en BD (Estado + UltConexion).
        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult<UsuarioResponseDto>> Logout()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(idStr, out var idUsuario))
                return Unauthorized(); // esto NO pasa por middleware (es una respuesta explícita)

            var result = await _autenticacionService.LogoutUsuario(idUsuario);
            return Ok(result);
        }
    }
}

