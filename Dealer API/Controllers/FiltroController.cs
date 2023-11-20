using Dealer_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dealer_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FiltroController : ControllerBase
    {
        public readonly DealerContext _dbContext;

        public FiltroController(DealerContext _context)
        {
            _dbContext = _context;
        }

        [HttpGet("Filtro-Vehiculo")]
        public async Task<IActionResult> FiltrarVehiculo(string filtro)
        {
            try
            {
                if (int.TryParse(filtro, out int año))
                {
                    var filtrarVehiculos = await _dbContext.Vehiculos
                        .Where(v => v.Tipo == filtro || v.Marca == filtro || v.Año == año || v.Modelo == filtro)
                        .ToListAsync();

                    return Ok(filtrarVehiculos);
                }
                else
                {
                    var filtrarVehiculos = await _dbContext.Vehiculos
                       .Where(v => v.Tipo == filtro || v.Marca == filtro || v.Modelo == filtro)
                       .ToListAsync();

                    return Ok(filtrarVehiculos);
                }
            }
            catch (Exception ex)
            {
                // Loggear el error o realizar acciones adicionales si es necesario
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor.");
            }
        }

        [HttpGet("FiltrarPorRangoPrecio")]
        public async Task<ActionResult<IEnumerable<Vehiculo>>> FiltrarPorPrecio(int precioMin, int precioMax)
        {
            try
            {
                var vehiculosFiltrados = await _dbContext.Vehiculos
                    .Where(v => v.Precio >= precioMin && v.Precio <= precioMax)
                    .ToListAsync();

                if (vehiculosFiltrados.Any())
                {
                    return Ok(vehiculosFiltrados);
                }
                else
                {
                    return NotFound($"No se encontraron vehículos dentro del rango de precios especificado ({precioMin} - {precioMax}).");
                }
            }
            catch (Exception ex)
            {
                // Manejar otras excepciones de manera genérica
                return StatusCode(500, $"Error en la consulta: {ex.Message}");
            }
        }

    }
}
