using Dealer_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Dealer_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransaccionesController : ControllerBase
    {
        public readonly DealerContext _dbContext;

        public TransaccionesController(DealerContext _context)
        {
            _dbContext = _context;
        }

        [HttpPost("realizar-venta")]
        public async Task<ActionResult> RealizarVenta(int vehiculoId, int compradorId)
        {
            try
            {
                // Verificar si el vehículo está disponible para la transacción
                var vehiculo = await _dbContext.Vehiculos.FindAsync(vehiculoId);
                if (vehiculo == null || vehiculo.Vendido)
                {
                    return NotFound("El vehículo no está disponible para la transacción.");
                }

                // Crear la transacción de venta
                var transaccionVenta = new Transaccion
                {
                    TipoTransaccionId = 1, // 1 para venta
                    Monto = vehiculo.Precio,
                    FechaTransaccion = DateTime.Now,
                    VehiculoId = vehiculoId,
                    ClienteId = compradorId
                };

                // Crear la transacción de compra
                var transaccionCompra = new Transaccion
                {
                    TipoTransaccionId = 2, // 2 para compra
                    Monto = vehiculo.Precio,
                    FechaTransaccion = DateTime.Now,
                    VehiculoId = vehiculoId,
                    ClienteId = compradorId
                };

                vehiculo.Vendido = true;

                _dbContext.Transaccions.AddRange(transaccionVenta, transaccionCompra);

                await _dbContext.SaveChangesAsync();

                return Ok(new { Venta = transaccionVenta, Compra = transaccionCompra });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("historial-transacciones")]
        public ActionResult<IEnumerable<Transaccion>> ObtenerHistorialTransacciones()
        {
            var historial = _dbContext.Transaccions
                .Include(t => t.Vehiculo)
                .Include(t => t.Cliente)
                .Include(t => t.TipoTransaccion)
                .ToList();

            var historialTransaccion = historial.Select(t => new Transaccion
            {
                Id = t.Id,
                TipoTransaccionId = t.TipoTransaccionId,
                Monto = t.Monto,
                FechaTransaccion = t.FechaTransaccion,
                Vehiculo = new Vehiculo
                {
                    Tipo = t.Vehiculo.Tipo,
                    Marca = t.Vehiculo.Marca,
                    Modelo = t.Vehiculo.Modelo,
                    Año = t.Vehiculo.Año,
                    Color = t.Vehiculo.Color,
                    Condicion = t.Vehiculo.Condicion,
                    Precio = t.Vehiculo.Precio,
                    Imagen = t.Vehiculo.Imagen,
                    Descripcion = t.Vehiculo.Descripcion,
                    PropietarioId = t.Vehiculo.PropietarioId
                },
                Cliente = new Usuario
                {
                    Nombre = t.Cliente?.Nombre,
                    Apellidos = t.Cliente?.Apellidos,
                    Correo = t.Cliente?.Correo,
                    Celular = t.Cliente?.Celular
                }
            }).ToList();

            return Ok(historialTransaccion);
        }


        [HttpGet("historial-transacciones-usuario/{idUsuario}")]
        public ActionResult<IEnumerable<Transaccion>> ObtenerHistorialTransaccionesUsuario(int idUsuario)
        {
            try
            {
                // Obtener el historial de transacciones para el usuario específico
                var historial = _dbContext.Transaccions
                    .Include(t => t.Vehiculo)
                    .Include(t => t.Cliente)
                    .Include(t => t.TipoTransaccion)
                    .Where(t => t.ClienteId == idUsuario || t.Vehiculo.PropietarioId == idUsuario)  // Filtrar por el ID del usuario
                    .ToList();

                if (!historial.Any())
                {
                    return NotFound($"El usuario con ID {idUsuario} no ha realizado ninguna transacción.");
                }

                // Mapear y proyectar los datos según tus necesidades
                var historialTransaccion = historial.Select(t => new Transaccion
                {
                    Id = t.Id,
                    TipoTransaccionId = t.TipoTransaccionId,
                    Monto = t.Monto,
                    FechaTransaccion = t.FechaTransaccion,
                    Vehiculo = new Vehiculo
                    {
                        Tipo = t.Vehiculo.Tipo,
                        Marca = t.Vehiculo.Marca,
                        Modelo = t.Vehiculo.Modelo,
                        Año = t.Vehiculo.Año,
                        Color = t.Vehiculo.Color,
                        Condicion = t.Vehiculo.Condicion,
                        Precio = t.Vehiculo.Precio,
                        Imagen = t.Vehiculo.Imagen,
                        Descripcion = t.Vehiculo.Descripcion,
                        PropietarioId = t.Vehiculo.PropietarioId
                    },
                    Cliente = new Usuario
                    {
                        Nombre = t.Cliente?.Nombre,
                        Apellidos = t.Cliente?.Apellidos,
                        Correo = t.Cliente?.Correo,
                        Celular = t.Cliente?.Celular
                    }
                }).ToList();

                return Ok(historialTransaccion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error en la consulta: {ex.Message}");
            }
        }

    }


}
