using Dealer_API.Models;
using Dealer_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Dealer_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class bUsuarioController : ControllerBase
    {
        public readonly DealerContext _dbContext;

        public bUsuarioController(DealerContext _context )
        {
            _dbContext = _context;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("Lista-Usuarios")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            try
            {
                var usuarios = await _dbContext.Usuarios.ToListAsync();

                if (usuarios == null || usuarios.Count == 0)
                {
                    return NotFound("No se encontraron usuarios.");
                }

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Get-Usuario")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            try
            {
                var usuario = await _dbContext.Usuarios.FindAsync(id);

                if (usuario == null)
                {
                    return NotFound($"No se encontró ningún usuario con el ID {id}.");
                }

                return Ok(usuario); 
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor.");
            }
        }

        [HttpPut("Editar-Usuario/{id}")]
        public async Task<IActionResult> EditarUsuario(int id, UsuarioDTO usuarioDTO)
        {
            try
            {
                Encriptar encriptar = new Encriptar();

                var usuario = await _dbContext.Usuarios.FindAsync(id);

                if (usuario == null)
                {
                    return NotFound($"No se encontró ningún usuario con el ID {id}.");
                }

                usuario.Nombre = usuarioDTO.Nombre;
                usuario.Apellidos = usuarioDTO.Apellidos;
                usuario.Correo = usuarioDTO.Correo;
                usuario.Contraseña = encriptar.ConvertirSha256(usuarioDTO.Contraseña);
                usuario.Celular = usuarioDTO.Celular;

                _dbContext.Usuarios.Update(usuario);
                await _dbContext.SaveChangesAsync();

                return Ok(new { mensaje = "Usuario editado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Eliminar-Usuario/{id}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            try
            {
                var usuario = await _dbContext.Usuarios.FindAsync(id);

                if (usuario == null)
                {
                    return NotFound($"No se encontró ningún usuario con el ID {id}.");
                }

                var vehiculos = await _dbContext.Vehiculos.Where(v => v.PropietarioId == id).ToListAsync();

                _dbContext.Vehiculos.RemoveRange(vehiculos);
                _dbContext.Usuarios.Remove(usuario);
                await _dbContext.SaveChangesAsync();

                return Ok(new { mensaje = "Usuario y sus vehículos eliminados exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor.");
            }
        }
    }
}

