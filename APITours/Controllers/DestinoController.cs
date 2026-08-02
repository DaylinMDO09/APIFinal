using Microsoft.AspNetCore.Mvc;
using APITours.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace APITours.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class DestinoController : ControllerBase
    {
        private readonly AppDbContext _context;
        
        public DestinoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("ObtenerDestinos")]
        public async Task<ActionResult> ObtenerDestinos()
        {
            var destinos = await _context.Destino.ToListAsync();
            if (destinos.Count == 0)
            {
                return BadRequest(new { mensaje = "No se encuentran destinos en la base de datos." }); 
            }
            return Ok(destinos);
        }
        [HttpGet("ObtenerDestino/{id}")]
        public async Task<IActionResult> ObtenerDestino(int id)
        {
            var destino = await _context.Destino.FindAsync(id);
            if (destino == null)
            {
                return NotFound(new { mensaje = "Destino no encontrado" });
            }
            return Ok(destino);
        }
        [HttpPost("AgregarDestino")]
        public async Task<IActionResult> AgregarDestino([FromBody] DestinoModel destino)
        {
            var pais = await _context.Pais.FindAsync(destino.IdPais);
            if (pais == null)
            {
                return BadRequest(new { mensaje = "El país especificado no existe" });
            }
            _context.Destino.Add(destino);
            await _context.SaveChangesAsync();
            return Ok(destino);
        }
        [HttpPut("ActualizarDestino/{id}")]
        public async Task<IActionResult> ActualizarDestino(int id, [FromBody] DestinoModel destino)
        {
            if (id != destino.IdDestino)
            {
                return BadRequest(new { mensaje = "El ID del destino no coincide" });
            }
            var pais = await _context.Pais.FindAsync(destino.IdPais);
            if (pais == null)
            {
                return BadRequest(new { mensaje = "El país especificado no existe" });
            }
            try { 
                _context.Entry(destino).State = EntityState.Modified;            
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DestinoBD(id))
                {
                    return NotFound(new { mensaje = "Destino no encontrado" });
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        private bool DestinoBD(int id)
        {
            return _context.Destino.Any(e => e.IdDestino == id);
        }
        [HttpDelete("EliminarDestino/{id}")]
        public async Task<IActionResult> EliminarDestino(int id)
        {
            var destino = await _context.Destino.FindAsync(id);
            if (destino == null)
            {
                return NotFound(new { mensaje = "Destino no encontrado" });
            }
            _context.Destino.Remove(destino);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
