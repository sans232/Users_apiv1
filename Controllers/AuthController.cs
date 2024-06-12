using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI_Users.Models;
using WebAPI_Users.Services;

namespace WebAPI_Users.Controllers
{
    [Route("users/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginRequest request)
        {
            var user = await _authService.AuthenticateAsync(request.Correo, request.Clave);

            if (user == null)
            {
                var emailExists = await _authService.CheckEmailExistsAsync(request.Correo);
                if (!emailExists)
                {
                    return Unauthorized(new { message = "Correo incorrecto." });
                }

                var validPassword = await _authService.CheckPasswordAsync(request.Correo, request.Clave);
                if (!validPassword)
                {
                    return Unauthorized(new { message = "Clave incorrecta." });
                }

                var activeStatus = await _authService.CheckUserStatusAsync(request.Correo);
                if (!activeStatus)
                {
                    return Unauthorized(new { message = "El usuario está inactivo." });
                }
            }

            return Ok(new { message = "Inicio de sesión exitoso.", user });
        }
    }
}
