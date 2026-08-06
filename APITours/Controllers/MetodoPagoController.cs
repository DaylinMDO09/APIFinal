using Microsoft.AspNetCore.Mvc;
using APITours.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace APITours.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class MetodoPagoController : Controller
    {
        private readonly AppDbContext _context;
        public MetodoPagoController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("ObtenerMetodosPago")]
        public async Task<ActionResult> ObtenerMetodosPago()
        {
            var metodosPago = await _context.MetodoPago.ToListAsync();
            if (metodosPago.Count == 0)
            {
                return BadRequest(new { mensaje = "No se encuentran métodos de pago en la base de datos." });
            }
            return Ok(metodosPago);
        }
        [HttpGet("ObtenerMetodoPago/{id}")]
        public async Task<ActionResult> ObtenerMetodoPagoPorId(int id)
        {
            var metodoPago = await _context.MetodoPago.FindAsync(id);
            if (metodoPago == null)
            {
                return NotFound(new { Mensaje = "Método de pago no encontrado" });
            }
            return Ok(metodoPago);
        }
        [HttpPost("AgregarMetodoPago")]
        public async Task<ActionResult> AgregarMetodoPago([FromBody] MetodoPagoModel metodoPago)
        {
            _context.MetodoPago.Add(metodoPago);
            await _context.SaveChangesAsync();
            return Ok(metodoPago);
        }
        [HttpPut("ActualizarMetodoPago/{id}")]
        public async Task<ActionResult> ActualizarMetodoPago(int id, [FromBody] MetodoPagoModel metodoPago)
        {
            if (id != metodoPago.IdMetodoPago)
            {
                return BadRequest(new { mensaje = "El ID del método de pago no coincide" });
            }
            _context.Entry(metodoPago).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MetodoPagoExists(id))
                {
                    return NotFound(new { mensaje = "Método de pago no encontrado" });
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        private bool MetodoPagoExists(int id)
        {
            return _context.MetodoPago.Any(e => e.IdMetodoPago == id);
        }
        [HttpDelete("EliminarMetodoPago/{id}")]
        public async Task<ActionResult> EliminarMetodoPago(int id)
        {
            var metodoPago = await _context.MetodoPago.FindAsync(id);
            if (metodoPago == null)
            {
                return NotFound(new { mensaje = "Método de pago no encontrado" });
            }
            var reservaExistente = await _context.Reservas.AnyAsync(r => r.idMetodoPago == id);
            if (reservaExistente)
            {
                return BadRequest(new { mensaje = "No se puede eliminar el método de pago porque está asociado a una reserva." });
            }
            _context.MetodoPago.Remove(metodoPago);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
