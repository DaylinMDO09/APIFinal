using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APITours.Modelos;
using Microsoft.AspNetCore.Authorization;

namespace APITours.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaisesController : ControllerBase
    {
        private readonly AppDbContext _context;
        
        public PaisesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("ObtenerPaises")]
        public async Task<ActionResult> ObtenerPaises()
        {
            var paises = await _context.Pais.ToListAsync();
            return Ok(paises);
        }
        [HttpGet("ObtenerPais/{id}")]
        public async Task<IActionResult> ObtenerPais(int id)
        {
            var pais = await _context.Pais.FindAsync(id);
            if (pais == null)
            {
                return NotFound();
            }
            return Ok(pais);
        }
        [HttpPost("AgregarPais")]
        public async Task<IActionResult> AgregarPais([FromBody] PaisModel pais)
        {
            _context.Pais.Add(pais);
            await _context.SaveChangesAsync();
            return Ok(pais);
        }
        [HttpPut("ActualizarPais/{id}")]
        public async Task<IActionResult> ActualizarPais(int id, [FromBody] PaisModel pais)
        {
            if (id != pais.IdPais)
            {
                return BadRequest();
            }
            _context.Entry(pais).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("EliminarPais/{id}")]
        public async Task<IActionResult> EliminarPais(int id)
        {
            var pais = await _context.Pais.FindAsync(id);
            if (pais == null)
            {
                return NotFound();
            }
            var destinoExistente = await _context.Destino.AnyAsync(d => d.IdPais == id);
            if (destinoExistente)
            {
                return BadRequest(new { mensaje = "No se puede eliminar el país porque está asociado a un destino." });
            }
            var tourExistente = await _context.Tours.AnyAsync(t => t.idPais == id);
            if (tourExistente)
            {
                return BadRequest(new { mensaje = "No se puede eliminar el país porque está asociado a un tour." });
            }
            _context.Pais.Remove(pais);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
