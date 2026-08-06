using Microsoft.AspNetCore.Mvc;
using APITours.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace APITours.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriaController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("ObtenerCategorias")]
        public async Task<ActionResult> ObtenerCategorias()
        {
            var categorias = await _context.Categoria.ToListAsync();
            if (categorias.Count == 0)
            {
                return BadRequest(new { mensaje = "No se encuentran categorías en la base de datos." });
            }
            return Ok(categorias);
        }
        [HttpGet("ObtenerCategoria/{id}")]
        public async Task<ActionResult> ObtenerCategoriasPorId(int id)
        {
            var categoria = await _context.Categoria.FindAsync(id);
            if (categoria == null)
            {
                return NotFound(new { Mensaje = "Categoria no encontrada" });
            }
            return Ok(categoria);
        }
        [HttpPost("AgregarCategoria")]
        public async Task<ActionResult> AgregarCategoria([FromBody] CategoriaModel categoria)
        {
            _context.Categoria.Add(categoria);
            await _context.SaveChangesAsync();
            return Ok(categoria);
        }
        [HttpPut("ActualizarCategoria/{id}")]
        public async Task<ActionResult> ActualizarCategoria(int id, [FromBody] CategoriaModel categoria)
        {
            if (id != categoria.IdCategoria)
            {
                return BadRequest(new { mensaje = "El ID de la categoría no coincide" });
            }
            _context.Entry(categoria).State = EntityState.Modified;
            try 
            { 
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(id))
                {
                    return NotFound(new { mensaje = "Categoría no encontrada" });
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categoria.Any(e => e.IdCategoria == id);
        }

        [HttpDelete("EliminarCategoria/{id}")]
        public async Task<ActionResult> EliminarCategoria(int id)
        {
            var categoria = await _context.Categoria.FindAsync(id);
            if (categoria == null)
            {
                return NotFound(new { mensaje = "Categoría no encontrada" });
            }
            var tourExistente = await _context.Tours.AnyAsync(t => t.idCategoria == id);
            if (tourExistente)
            {
                return BadRequest(new { mensaje = "No se puede eliminar la categoría porque está asociada a un tour." });
            }
            _context.Categoria.Remove(categoria);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
