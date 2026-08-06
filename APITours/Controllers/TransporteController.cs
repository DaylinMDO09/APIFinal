using Microsoft.AspNetCore.Mvc;
using APITours.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace APITours.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class TransporteController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TransporteController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("ObtenerTransportes")]
        public async Task<ActionResult> ObtenerTransportes()
        {
            var transportes = await _context.Transporte.ToListAsync();
            if (transportes.Count == 0)
            {
                return BadRequest(new { mensaje = "No se encuentran transportes en la base de datos." });
            }
            return Ok(transportes);
        }
        [HttpGet("ObtenerTransporte/{id}")]
        public async Task<ActionResult> ObtenerTransportePorId(int id)
        {
            var transporte = await _context.Transporte.FindAsync(id);
            if (transporte == null)
            {
                return NotFound(new { Mensaje = "Transporte no encontrado" });
            }
            return Ok(transporte);
        }
        [HttpPost("AgregarTransporte")]
        public async Task<ActionResult> AgregarTransporte([FromBody] TransporteModel transporte)
        {
            _context.Transporte.Add(transporte);
            await _context.SaveChangesAsync();
            return Ok(transporte);
        }
        [HttpPut("ActualizarTransporte/{id}")]
        public async Task<ActionResult> ActualizarTransporte(int id, [FromBody] TransporteModel transporte)
        {
            if (id != transporte.IdTransporte)
            {
                return BadRequest(new { mensaje = "El ID del transporte no coincide" });
            }
            _context.Entry(transporte).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransporteExists(id))
                {
                    return NotFound(new { mensaje = "Transporte no encontrado" });
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        private bool TransporteExists(int id)
        {
            return _context.Transporte.Any(e => e.IdTransporte == id);
        }
        [HttpDelete("EliminarTransporte/{id}")]
        public async Task<ActionResult> EliminarTransporte(int id)
        {
            var transporte = await _context.Transporte.FindAsync(id);
            if (transporte == null)
            {
                return NotFound(new { mensaje = "Transporte no encontrado" });
            }
            var tourExistente = await _context.Tours.AnyAsync(t => t.idPais == id);
            if (tourExistente)
            {
                return BadRequest(new { mensaje = "No se puede eliminar el transporte porque está asociado a un tour." });
            }
            _context.Transporte.Remove(transporte);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
