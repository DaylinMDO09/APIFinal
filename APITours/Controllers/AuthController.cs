using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APITours.Modelos;
using APITours.Tokens;
using APITours.Modelos.Autenticacion;

namespace APITours.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ServicioJwt _servicioJwt;

        public AuthController(AppDbContext context, ServicioJwt servicioJwt)
        {
            _context = context;
            _servicioJwt = servicioJwt;
        }

        [HttpPost("LogIn")]
        public async Task<ActionResult<AutenticacionRespondida>> LogIn([FromBody] LogInRespondido usuario)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == usuario.NombreUsuario && u.ClaveUsuario == usuario.ClaveUsuario);
            if (user == null)
            {
                return Unauthorized("Usuario o contraseña incorrectos");
            }
            var token = _servicioJwt.GenerarToken(user);
            return Ok(new 
            {
                Token = token,
                Expira = DateTime.UtcNow.AddHours(1),
                NombreUsuario = user.NombreUsuario,
                Correo = user.CorreoUsuario

            });
        }

        [HttpPost("Registrar")]
        public async Task<ActionResult<AutenticacionRespondida>> Registrar([FromBody] RegistroRespondido usuario)
        {
            _context.Usuarios.Add(new UsuariosModel
            {
                NombreCompleto = usuario.NombreCompleto,
                NombreUsuario = usuario.NombreUsuario,
                CorreoUsuario = usuario.CorreoUsuario,
                ClaveUsuario = usuario.ClaveUsuario
            });
            await _context.SaveChangesAsync();
            return Ok("Usuario registrado correctamente");
        }
    }
}
