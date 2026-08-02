using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace APITours.Modelos
{
    public class ToursModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTour { get; set; }
        [Required(ErrorMessage = "El nombre del tour es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre del tour debe tener entre 3 y 100 caracteres.")]
        public string NombreTour { get; set; } = null!;
        public DateOnly FechaTour { get; set; }
        public TimeOnly HoraTour { get; set; }

        [Required(ErrorMessage = "El precio del tour es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio del tour debe ser mayor que cero.")]
        public decimal PrecioTour { get; set; }
        public decimal ImpuestoTour => PrecioTour * 0.18m; // 18% de impuesto
        public int DuracionTour { get; set; } // Duración en horas
        public DateTime FechaHoraFin => FechaTour.ToDateTime(HoraTour).AddHours(DuracionTour); // Fecha y hora de finalización del tour

        public string EstadoTour
        {
            get
            {
                var fechaHoraActual = DateTime.Now;
                var fechaHoraInicio = FechaTour.ToDateTime(HoraTour);
                if (fechaHoraActual < fechaHoraInicio)
                {
                    return "Tour Pendiente";
                }
                else if (fechaHoraActual >= fechaHoraInicio && fechaHoraActual <= FechaHoraFin)
                {
                    return "Tour En Curso";
                }
                else
                {
                    return "Tour Finalizado";
                }
            }
        }

        public int idPais { get; set; } = 0;
        public int idDestino { get; set; } = 0;
        public int idCategoria { get; set; } = 0;
        public int idGuiaTuristico { get; set; } = 0;
        public int idTransporte { get; set; } = 0;

    }
}
