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
            var tours = await _context.Tours.ToListAsync();
            if (tours.Count == 0)
            {
                return BadRequest(new { mensaje = "No se encuentran tours en la base de datos." });
            }
            return Ok(tours);
        }
        [HttpGet("ObtenerTour/{id}")]
        public async Task<ActionResult> ObtenerTourPorId(int id)
        {
            var tour = await _context.Tours.FindAsync(id);
            if (tour == null)
            {
                return NotFound(new { Mensaje = "Tour no encontrado" });
            }
            return Ok(tour);
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
            DateTime fechafinalTour = fechainicioTour.AddMinutes(tour.DuracionTour);

            var tourRepetido = _context.Tours.AsEnumerable().Any(
                t => {
                    DateTime fechaInicioExistente = t.FechaTour.ToDateTime(t.HoraTour);
                    DateTime fechaFinExistente = fechaInicioExistente.AddMinutes(t.DuracionTour);
                    return (fechainicioTour < fechaFinExistente && fechafinalTour > fechaInicioExistente);
                });
            if (tourRepetido)
            {
                return BadRequest(new { Mensaje = "Ya existe un tour programado en esas fechas. Puede programa otro tour para una otra fecha." });
            }
            _context.Tours.Add(tour);
            await _context.SaveChangesAsync();
            return Ok(tour);
            
        }  
    }
}
