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
    }
}
