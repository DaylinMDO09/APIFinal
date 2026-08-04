using Microsoft.AspNetCore.Mvc;
using APITours.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace APITours.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class GuiaTuristicoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GuiaTuristicoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("ObtenerGuias")]
        public async Task<ActionResult> ObtenerGuias()
        {
            var guias = await _context.GuiaTuristico.ToListAsync();
            if (guias.Count == 0)
            {
                return BadRequest(new { mensaje = "No se encuentran guías turísticos en la base de datos." });
            }
            return Ok(guias);
        }

        [HttpGet("ObtenerGuia/{id}")]
        public async Task<ActionResult> ObtenerGuiaPorId(int id)
        {
            var guia = await _context.GuiaTuristico.FindAsync(id);
            if (guia == null)
            {
                return NotFound(new { Mensaje = "Guía turístico no encontrado" });
            }
            return Ok(guia);
        }

        [HttpPost("AgregarGuia")]
        public async Task<ActionResult> AgregarGuia([FromBody] GuiaTuristicoModel guia)
        {
            _context.GuiaTuristico.Add(guia);
            await _context.SaveChangesAsync();
            return Ok(guia);
        }
        [HttpPut("ActualizarGuia/{id}")]
        public async Task<ActionResult> ActualizarGuia(int id, [FromBody] GuiaTuristicoModel guia)
        {
            if (id != guia.IdGuiaTuristico)
            {
                return BadRequest(new { mensaje = "El ID de la guía turístico no coincide" });
            }
            _context.Entry(guia).State = EntityState.Modified;
            try 
            { 
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GuiaExists(id))
                {
                    return NotFound(new { mensaje = "Guía turístico no encontrado" });
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        private bool GuiaExists(int id)
        {
            return _context.GuiaTuristico.Any(e => e.IdGuiaTuristico == id);
        }

        [HttpDelete("EliminarGuia/{id}")]
        public async Task<ActionResult> EliminarGuia(int id)
        {
            var guia = await _context.GuiaTuristico.FindAsync(id);
            if (guia == null)
            {
                return NotFound(new { mensaje = "Guía turístico no encontrado" });
            }
            _context.GuiaTuristico.Remove(guia);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
