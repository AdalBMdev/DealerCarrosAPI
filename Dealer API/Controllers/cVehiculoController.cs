using Dealer_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Dealer_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class cVehiculoController : ControllerBase
    {

        public readonly DealerContext _dbContext;
        private int? idUsuario; // ??????

        public cVehiculoController(DealerContext _context)
        {
            _dbContext = _context;
        }

        [HttpPost("Añadir-Vehiculo")]
        public async Task<ActionResult> AñadirVehiculo([FromBody] VehiculoDTO vehiculoDTO)
        {
            try
            {
                var vehiculo = new Vehiculo
                {
                    Tipo = vehiculoDTO.Tipo,
                    Marca = vehiculoDTO.Marca,
                    Modelo = vehiculoDTO.Modelo,
                    Año = vehiculoDTO.Año,
                    Color = vehiculoDTO.Color,
                    Condicion = vehiculoDTO.Condicion,
                    Precio = vehiculoDTO.Precio,
                    Imagen = vehiculoDTO.Imagen,
                    Descripcion = vehiculoDTO.Descripcion,
                    PropietarioId = vehiculoDTO.PropietarioId,
                    Vendido = vehiculoDTO.Vendido
                };

                _dbContext.Vehiculos.Add(vehiculo);
                await _dbContext.SaveChangesAsync();

                // Devuelve una respuesta descriptiva con un código de estado y un mensaje
                return CreatedAtAction(nameof(GetVehiculo), new { id = vehiculo.Id }, new { mensaje = "Vehículo añadido exitosamente." });
            }
            catch (Exception ex)
            {
                // Loggear el error o realizar acciones adicionales si es necesario
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor.");
            }
        }

        [HttpGet("Catalogo")]
        public async Task<IActionResult> Catalogo()
        {
            try
            {
                var vehiculos = await _dbContext.Vehiculos.ToListAsync();
                return Ok(vehiculos);
            }
            catch (Exception ex)
            {
                // Loggear el error o realizar acciones adicionales si es necesario
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor.");
            }
        }

        [HttpGet("Catalogo-Usuario")]
        public async Task<IActionResult> GetVehiculoUsuario(int idUsuario)
        {
            try
            {
                var usuario = await _dbContext.Usuarios.FindAsync(idUsuario);

                if (usuario == null)
                {
                    return NotFound($"No se encontró ningún usuario con el ID {idUsuario}.");
                }

                var vehiculos = await _dbContext.Vehiculos
                    .Where(vehiculo => vehiculo.PropietarioId == idUsuario && !vehiculo.Vendido)
                    .ToListAsync();

                return Ok(vehiculos);
            }
            catch (Exception ex)
            {
                // Loggear el error o realizar acciones adicionales si es necesario
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor.");
            }
        }

        [HttpGet("Vehiculos-Vendidos")]
        public async Task<IActionResult> GetVehiculosVendidos()
        {
            try
            {
                var vehiculosVendidos = await _dbContext.Vehiculos
                    .Where(vehiculo => vehiculo.Vendido)
                    .ToListAsync();

                return Ok(vehiculosVendidos);
            }
            catch (Exception ex)
            {
                // Loggear el error o realizar acciones adicionales si es necesario
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor.");
            }
        }

        [HttpGet("Vehiculos-Vendidos/{userId}")]
        public async Task<IActionResult> GetVehiculosVendidosPorUsuario(int userId)
        {
            try
            {
                // Obtener vehículos vendidos asociados al usuario con el ID proporcionado
                var vehiculosVendidos = await _dbContext.Vehiculos
                    .Where(vehiculo => vehiculo.Vendido && vehiculo.PropietarioId == userId)
                    .ToListAsync();

                return Ok(vehiculosVendidos);
            }
            catch (Exception ex)
            {
                // Loggear el error o realizar acciones adicionales si es necesario
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor.");
            }
        }



        [HttpGet("Obtener-VehiculoID")]
        public async Task<IActionResult> GetVehiculo(int id)
        {
            try
            {
                var vehiculo = await _dbContext.Vehiculos.FindAsync(id);

                if (vehiculo == null)
                {
                    return NotFound($"No se encontró ningún vehículo con el ID {id}.");
                }

                return Ok(vehiculo);
            }
            catch (Exception ex)
            {
                // Loggear el error o realizar acciones adicionales si es necesario
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor.");
            }
        }

        [HttpPut("Editar-Vehiculo/{id}")]
        public async Task<IActionResult> EditarVehiculo(int id, VehiculoDTO vehiculoDTO)
        {
            try
            {
                var vehiculo = await _dbContext.Vehiculos.FindAsync(id);

                if (vehiculo == null)
                {
                    return NotFound($"No se encontró ningún vehículo con el ID {id}.");
                }

                // Actualizar las propiedades del vehículo con los valores del DTO
                vehiculo.Tipo = vehiculoDTO.Tipo;
                vehiculo.Marca = vehiculoDTO.Marca;
                vehiculo.Modelo = vehiculoDTO.Modelo;
                vehiculo.Año = vehiculoDTO.Año;
                vehiculo.Color = vehiculoDTO.Color;
                vehiculo.Condicion = vehiculoDTO.Condicion;
                vehiculo.Precio = vehiculoDTO.Precio;
                vehiculo.Imagen = vehiculoDTO.Imagen;
                vehiculo.Descripcion = vehiculoDTO.Descripcion;
                vehiculo.Vendido = vehiculoDTO.Vendido;

                // Guardar los cambios en la base de datos
                _dbContext.Vehiculos.Update(vehiculo);
                await _dbContext.SaveChangesAsync();

                // Devuelve un OkObjectResult con un mensaje de éxito
                return Ok(new { mensaje = "Vehículo editado exitosamente" });
            }
            catch (Exception ex)
            {
                // Loggear el error o realizar acciones adicionales si es necesario
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor.");
            }
        }


        [HttpDelete("Eliminar-Vehiculo/{id}")]
        public async Task<IActionResult> EliminarVehiculo(int id)
        {
            
                var vehiculo = await _dbContext.Vehiculos.FindAsync(id);

                if (vehiculo == null)
                {
                    return NotFound($"No se encontró ningún vehículo con el ID {id}.");
                }

                // Eliminar manualmente las transacciones asociadas
                var transacciones = await _dbContext.Transaccions
                    .Where(t => t.VehiculoId == id)
                    .ToListAsync();

                _dbContext.Transaccions.RemoveRange(transacciones);

                _dbContext.Vehiculos.Remove(vehiculo);
                await _dbContext.SaveChangesAsync();

                // Agrega un mensaje de éxito
                return Ok("Se ha eliminado el vehiculo");
          
        }

    }
}