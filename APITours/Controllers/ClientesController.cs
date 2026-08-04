using Microsoft.AspNetCore.Mvc;
using APITours.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace APITours.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ClientesController : Controller
    {
        private readonly AppDbContext _context;
        public ClientesController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("ObtenerClientes")]
        public async Task<ActionResult> ObtenerClientes()
        {
            var clientes = await _context.Clientes.ToListAsync();
            if (clientes.Count == 0)
            {
                return BadRequest(new { mensaje = "No se encuentran clientes en la base de datos." });
            }
            return Ok(clientes);
        }
        [HttpGet("ObtenerCliente/{id}")]
        public async Task<ActionResult> ObtenerClientePorId(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound(new { Mensaje = "Cliente no encontrado" });
            }
            return Ok(cliente);
        }
        [HttpPost("AgregarCliente")]
        public async Task<ActionResult> AgregarCliente([FromBody] ClientesModel cliente)
        {
            var clienteExistente = await _context.Clientes.FirstOrDefaultAsync(c => c.CorreoCliente == cliente.CorreoCliente || c.TelefonoCliente == cliente.TelefonoCliente);
            
            if (clienteExistente != null)
            {
                return BadRequest(new { mensaje = "Ya existe un cliente con las mismas credenciales" });
            }
            else
            {
                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();
                return Ok(cliente);
            }
        }
        [HttpPut("ActualizarCliente/{id}")]
        public async Task<ActionResult> ActualizarCliente(int id, [FromBody] ClientesModel cliente)
        {
            var clienteExistente = await _context.Clientes.FirstOrDefaultAsync(c => c.CorreoCliente == cliente.CorreoCliente || c.TelefonoCliente == cliente.TelefonoCliente && c.IdCliente != id);
            if (clienteExistente != null) { 
                return BadRequest(new { mensaje = "Ya existe un cliente con las mismas credenciales" });
            }

            if (id != cliente.IdCliente)
            {
                return BadRequest(new { mensaje = "El ID del cliente no coincide" });
            }
            _context.Entry(cliente).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
                {
                    return NotFound(new { mensaje = "Cliente no encontrado" });
                }
                else
                {
                    throw;
                }
            }
            return Ok(cliente);
        }
        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.IdCliente == id);
        }
        [HttpDelete("EliminarCliente/{id}")]
        public async Task<ActionResult> EliminarCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound(new { mensaje = "Cliente no encontrado" });
            }
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return NoContent();

        }
    }
}
