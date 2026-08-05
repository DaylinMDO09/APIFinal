using Microsoft.AspNetCore.Mvc;
using APITours.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace APITours.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ToursController : ControllerBase
    {
       private readonly AppDbContext _context;
        public ToursController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("ObtenerTours")]
        public async Task<ActionResult> ObtenerTours()
        {
            var tours = await _context.Tours.Include(t => t.Pais)
                                            .Include(t => t.Destino)
                                            .Include(t => t.Categoria)
                                            .Include(t => t.GuiaTuristico)
                                            .Include(t => t.Transporte)
                                            .ToListAsync();
            if (tours.Count == 0)
            {
                return BadRequest(new { mensaje = "No se encuentran tours en la base de datos." });
            }
            
            var resultadotours = tours.Select(t => new
            {
                t.IdTour,
                t.NombreTour,
                t.FechaTour,
                t.HoraTour,
                t.PrecioTour,
                t.ImpuestoTour,
                t.PrecioTotalTour,
                t.DuracionTour,
                t.FechaHoraFin,
                t.EstadoTour,
                t.Pais!.NombrePais,
                t.Destino!.NombreDestino,
                t.Categoria!.NombreCategoria,
                t.GuiaTuristico!.NombreGuiaTuristico,
                t.Transporte!.TipoTransporte
            });

            return Ok(resultadotours);
        }
        [HttpGet("ObtenerTour/{id}")]
        public async Task<ActionResult> ObtenerTourPorId(int id)
        {
            var tours = await _context.Tours.Include(t => t.Pais)
                                            .Include(t => t.Destino)
                                            .Include(t => t.Categoria)
                                            .Include(t => t.GuiaTuristico)
                                            .Include(t => t.Transporte)
                                            .Where(t => t.IdTour == id)
                                            .ToListAsync();
            if (tours == null)
            {
                return NotFound(new { Mensaje = "Tour no encontrado" });
            }
            var resultadotours = tours.Select(t => new
            {
                t.IdTour,
                t.NombreTour,
                t.FechaTour,
                t.HoraTour,
                t.PrecioTour,
                t.ImpuestoTour,
                t.PrecioTotalTour,
                t.DuracionTour,
                t.FechaHoraFin,
                t.EstadoTour,
                t.Pais!.NombrePais,
                t.Destino!.NombreDestino,
                t.Categoria!.NombreCategoria,
                t.GuiaTuristico!.NombreGuiaTuristico,
                t.Transporte!.TipoTransporte
            });

            return Ok(resultadotours);
        }
        [HttpPost("AgregarTour")]
        public async Task<ActionResult> AgregarTour(ToursModel tour)
        {
            DateTime fechahoraTour = new DateTime(tour.FechaTour.Year, tour.FechaTour.Month, tour.FechaTour.Day, tour.HoraTour.Hour, tour.HoraTour.Minute, tour.HoraTour.Second);
            if (fechahoraTour < DateTime.Now)
            {
                return BadRequest(new { Mensaje = "La fecha y hora del tour no puede ser anterior a la fecha y hora actual." });
            }
            DateTime fechainicioTour = tour.FechaTour.ToDateTime(tour.HoraTour);
            DateTime fechafinalTour = fechainicioTour.AddHours(tour.DuracionTour);

            var tourRepetido = _context.Tours.AsEnumerable().Any(
                t => {
                    DateTime fechaInicioExistente = t.FechaTour.ToDateTime(t.HoraTour);
                    DateTime fechaFinExistente = fechaInicioExistente.AddHours(t.DuracionTour);
                    return (fechainicioTour < fechaFinExistente && fechafinalTour > fechaInicioExistente);
                });
            if (tourRepetido)
            {
                return BadRequest(new { Mensaje = "Ya existe un tour programado en esas fechas. Puede programa otro tour para otra fecha." });
            }
            var pais = await _context.Pais.AnyAsync(p => p.IdPais == tour.idPais);
            if (!pais)
            {
                return BadRequest(new { Mensaje = "El país especificado no existe." });
            }
            var destino = await _context.Destino.AnyAsync(d => d.IdDestino == tour.idDestino);
            if (!destino)
            {
                return BadRequest(new { Mensaje = "El destino especificado no existe." });
            }
            var categoria = await _context.Categoria.AnyAsync(c => c.IdCategoria == tour.idCategoria);
            if (!categoria)
            {
                return BadRequest(new { Mensaje = "La categoría especificada no existe." });
            }
            var guia = await _context.GuiaTuristico.AnyAsync(g => g.IdGuiaTuristico == tour.idGuiaTuristico);
            if (!guia)
            {
                return BadRequest(new { Mensaje = "El guía turístico especificado no existe." });
            }
            var transporte = await _context.Transporte.AnyAsync(t => t.IdTransporte == tour.idTransporte);
            if (!transporte)
            {
                return BadRequest(new { Mensaje = "El transporte especificado no existe." });
            }
            var destinoValido = await _context.Destino.AnyAsync(d => d.IdDestino == tour.idDestino && d.IdPais == tour.idPais);
            if (!destinoValido)
            {
                return BadRequest(new { Mensaje = "El destino especificado no pertenece al país especificado." });
            }
            _context.Tours.Add(tour);
            await _context.SaveChangesAsync();
            return Ok(tour);
            
        }
        [HttpPut("ActualizarTour/{id}")]
        public async Task<ActionResult> ActualizarTour(int id, ToursModel tour)
        {
            if (id != tour.IdTour)
            {
                return BadRequest(new { Mensaje = "El ID del tour no coincide" });
            }
            DateTime fechahoraTour = new DateTime(tour.FechaTour.Year, tour.FechaTour.Month, tour.FechaTour.Day, tour.HoraTour.Hour, tour.HoraTour.Minute, tour.HoraTour.Second);
            if (fechahoraTour < DateTime.Now)
            {
                return BadRequest(new { Mensaje = "La fecha y hora del tour no puede ser anterior a la fecha y hora actual." });
            }
            DateTime fechainicioTour = tour.FechaTour.ToDateTime(tour.HoraTour);
            DateTime fechafinalTour = fechainicioTour.AddHours(tour.DuracionTour);

            var tourRepetido = _context.Tours.AsEnumerable().Any(
                t => {
                    DateTime fechaInicioExistente = t.FechaTour.ToDateTime(t.HoraTour);
                    DateTime fechaFinExistente = fechaInicioExistente.AddHours(t.DuracionTour);
                    return (fechainicioTour < fechaFinExistente && fechafinalTour > fechaInicioExistente);
                });
            if (tourRepetido)
            {
                return BadRequest(new { Mensaje = "Ya existe un tour programado en esas fechas. Puede programa otro tour para otra fecha." });
            }
            var pais = await _context.Pais.AnyAsync(p => p.IdPais == tour.idPais);
            if (!pais)
            {
                return BadRequest(new { Mensaje = "El país especificado no existe." });
            }
            var destino = await _context.Destino.AnyAsync(d => d.IdDestino == tour.idDestino);
            if (!destino)
            {
                return BadRequest(new { Mensaje = "El destino especificado no existe." });
            }
            var categoria = await _context.Categoria.AnyAsync(c => c.IdCategoria == tour.idCategoria);
            if (!categoria)
            {
                return BadRequest(new { Mensaje = "La categoría especificada no existe." });
            }
            var guia = await _context.GuiaTuristico.AnyAsync(g => g.IdGuiaTuristico == tour.idGuiaTuristico);
            if (!guia)
            {
                return BadRequest(new { Mensaje = "El guía turístico especificado no existe." });
            }
            var transporte = await _context.Transporte.AnyAsync(t => t.IdTransporte == tour.idTransporte);
            if (!transporte)
            {
                return BadRequest(new { Mensaje = "El transporte especificado no existe." });
            }
            var destinoValido = await _context.Destino.AnyAsync(d => d.IdDestino == tour.idDestino && d.IdPais == tour.idPais);
            if (!destinoValido)
            {
                return BadRequest(new { Mensaje = "El destino especificado no pertenece al país especificado." });
            }

            var tourExistente = await _context.Tours.FindAsync(id);
            if (tourExistente.EstadoTour == "Tour En Curso" || tourExistente.EstadoTour == "Tour Finalizado")
            {
                return BadRequest(new { Mensaje = "No es posible modificar los datos de este tour" });
            }

            _context.Entry(tour).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TourExists(id))
                {
                    return NotFound(new { Mensaje = "Tour no encontrado" });
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        private bool TourExists(int id)
        {
            return _context.Tours.Any(e => e.IdTour == id);
        }
        [HttpDelete("EliminarTour/{id}")]
        public async Task<ActionResult> EliminarTour(int id)
        {
            var tour = await _context.Tours.FindAsync(id);
            if (tour == null)
            {
                return NotFound(new { Mensaje = "Tour no encontrado" });
            }
            if (tour.EstadoTour == "Tour En Curso" || tour.EstadoTour == "Tour Finalizado")
            {
                return BadRequest(new { Mensaje = "No es posible eliminar este tour" });
            }
            var reservasExistentes = await _context.Reservas.AnyAsync(r => r.idTour == id);
            if (reservasExistentes)
            {
                return BadRequest(new { Mensaje = "No es posible eliminar el tour porque tiene reservas asociadas." });
            }
            _context.Tours.Remove(tour);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}