using Microsoft.AspNetCore.Mvc;
using APITours.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace APITours.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ReservasController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ReservasController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("ObtenerReservas")]
        public async Task<ActionResult> ObtenerReservas()
        {
            var reservas = await _context.Reservas.Include(r => r.Tour)
                                                  .Include(r => r.Cliente)
                                                  .Include(r => r.MetodoPago)
                                                  .ToListAsync();
            if (reservas.Count == 0)
            {
                return NotFound("No se encontraron reservas en la base de datos.");
            }

            var resultadoreservas = reservas.Select(r => new
            {
                r.IdReserva,
                r.FechaReserva,
                Cliente = new
                {
                    r.Cliente!.IdCliente,
                    r.Cliente!.NombreCompleto,
                    r.Cliente!.CorreoCliente,
                    r.Cliente!.TelefonoCliente
                },
                Tour = new
                {
                    r.Tour!.IdTour,
                    r.Tour!.NombreTour,
                    r.Tour!.FechaTour,
                    r.Tour!.HoraTour,
                    r.Tour!.PrecioTour,
                    r.Tour!.ImpuestoTour,
                    r.Tour!.PrecioTotalTour,
                    r.Tour!.DuracionTour,
                    r.Tour!.EstadoTour,
                    r.Tour!.Pais!.NombrePais,
                    r.Tour!.Destino!.NombreDestino,
                    r.Tour!.Categoria!.NombreCategoria,
                    r.Tour!.GuiaTuristico!.NombreGuiaTuristico,
                    r.Tour!.Transporte!.TipoTransporte
                },
                r.MetodoPago!.NombreMetodoPago
            });
            return Ok(resultadoreservas);
        }
        [HttpGet("ObtenerReservaPorId/{id}")]
        public async Task<ActionResult> ObtenerReservaPorId(int id)
        {
            var reserva = await _context.Reservas.Include(r => r.Tour)
                                                 .Include(r => r.Cliente)
                                                 .Include(r => r.MetodoPago)
                                                 .FirstOrDefaultAsync(r => r.IdReserva == id);
            if (reserva == null)
            {
                return NotFound($"No se encontró ninguna reserva con el ID {id}.");
            }
            var resultadoreserva = new
            {
                reserva.IdReserva,
                reserva.FechaReserva,
                Cliente = new
                {
                    reserva.Cliente!.IdCliente,
                    reserva.Cliente!.NombreCompleto,
                    reserva.Cliente!.CorreoCliente,
                    reserva.Cliente!.TelefonoCliente
                },
                Tour = new
                {
                    reserva.Tour!.IdTour,
                    reserva.Tour!.NombreTour,
                    reserva.Tour!.FechaTour,
                    reserva.Tour!.HoraTour,
                    reserva.Tour!.PrecioTour,
                    reserva.Tour!.ImpuestoTour,
                    reserva.Tour!.PrecioTotalTour,
                    reserva.Tour!.DuracionTour,
                    reserva.Tour!.EstadoTour,
                    reserva.Tour!.Pais!.NombrePais,
                    reserva.Tour!.Destino!.NombreDestino,
                    reserva.Tour!.Categoria!.NombreCategoria,
                    reserva.Tour!.GuiaTuristico!.NombreGuiaTuristico,
                    reserva.Tour!.Transporte!.TipoTransporte

                }
            };
            return Ok(resultadoreserva);
        }
        [HttpPost("CrearReserva")]
        public async Task<ActionResult> CrearReserva([FromBody] ReservasModel reserva)
        {
            DateTime fechaReserva = new DateTime(reserva.FechaReserva.Year, reserva.FechaReserva.Month, reserva.FechaReserva.Day);
            if (fechaReserva < DateTime.Today || fechaReserva > DateTime.Today)
            {
                return BadRequest("La fecha de la reserva no puede ser anterior ni posterior a la fecha actual.");
            }

            var clienteExistente = await _context.Clientes.AnyAsync(c => c.IdCliente == reserva.idCliente);
            if (!clienteExistente)
            {
                return BadRequest($"No se encontró ningún cliente con el ID {reserva.idCliente}.");
            }
            var TourExistente = await _context.Tours.AnyAsync(t => t.IdTour == reserva.idTour);
            if (!TourExistente)
            {
                return BadRequest($"No se encontró ningún tour con el ID {reserva.idTour}.");
            }
            var estadoTour = await _context.Tours.Where(t => t.IdTour == reserva.idTour).Select(t => t.EstadoTour).FirstOrDefaultAsync();
            if (estadoTour != "Tour En Curso" || estadoTour != "Tour Finalizado")
            {
                return BadRequest($"El tour con el ID {reserva.idTour} no está disponible para reservas.");
            }
            var MetodoPagoExistente = await _context.MetodoPago.AnyAsync(m => m.IdMetodoPago == reserva.idMetodoPago);
            if (!MetodoPagoExistente)
            {
                return BadRequest($"No se encontró ningún método de pago con el ID {reserva.idMetodoPago}.");
            }

            reserva.FechaReserva = DateTime.Now;

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();
            return Ok(reserva);
        }
    }
}