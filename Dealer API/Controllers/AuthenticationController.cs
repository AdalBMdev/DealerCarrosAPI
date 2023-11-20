using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dealer_API.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Dealer_API.Services;

namespace Dealer_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly DealerContext _context;

        private readonly string Key;

        Encriptar encriptar = new Encriptar();

        public AuthenticationController(IConfiguration config, DealerContext context)
        {
            //Key = config.GetSection("Jwt").GetSection("Key").ToString();
            Key = config["Jwt:Key"].ToString();
            _context = context;
        }

        [HttpPost]
        [Route("Registrar-Usuarios")]
        public IActionResult Registrar(UsuarioDTO usuarioDTO)
        {
            try
            {
                int rolId = 2;

                // Verificar si el correo ya está registrado
                if (_context.Usuarios.Any(usuario => usuario.Correo == usuarioDTO.Correo))
                {
                    return BadRequest("El correo ya está registrado.");
                }

                // Convertir la contraseña a SHA256
                var claveEncriptada = encriptar.ConvertirSha256(usuarioDTO.Contraseña);

                // Crear un nuevo usuario
                var nuevoUsuario = new Usuario
                {
                    Nombre = usuarioDTO.Nombre,
                    Apellidos = usuarioDTO.Apellidos,
                    Correo = usuarioDTO.Correo,
                    Contraseña = claveEncriptada,
                    Celular = usuarioDTO.Celular,
                    RolId = rolId // Asignar el rol proporcionado en el parámetro
                };

                // Agregar el usuario a la base de datos
                _context.Usuarios.Add(nuevoUsuario);
                _context.SaveChanges();

                // Devolver un mensaje de éxito
                return Created("Usuario registrado exitosamente.", null);
            }
            catch (Exception ex)
            {
                // Loggear el error o realizar acciones adicionales si es necesario
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor.");
            }
        }



        [HttpPost]
        [Route("Validar")]
        public IActionResult Validar(string correo, string contraseña)
        {
            var claveEncriptada = encriptar.ConvertirSha256(contraseña);

            var usuario = _context.Usuarios
                .Include(u => u.Rol)
                .Where(u => u.Correo == correo && u.Contraseña == claveEncriptada)
                .FirstOrDefault();

            if (usuario != null)
            {
                var keyBytes = Encoding.UTF8.GetBytes(Key);
                var claims = new ClaimsIdentity();

                // Añadir el ID del usuario como un nuevo claim
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()));

                claims.AddClaim(new Claim(ClaimTypes.Name, correo));

                var rolNombre = _context.Rols.Where(r => r.RolId == usuario.RolId).Select(r => r.Nombre).FirstOrDefault();
                claims.AddClaim(new Claim(ClaimTypes.Role, rolNombre));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddMinutes(45),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

                string tokenCreado = tokenHandler.WriteToken(tokenConfig);

                return StatusCode(StatusCodes.Status200OK, new { token = tokenCreado });
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { mensaje = "Usuario o contraseña incorrectos" });
            }
        }

    }
}
