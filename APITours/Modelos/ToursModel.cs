using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace APITours.Modelos
{
    public class ToursModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTour { get; set; }
        [Required(ErrorMessage = "El nombre del tour es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre del tour debe tener entre 3 y 100 caracteres.")]
        [Column("NOMBRE")]
        public string NombreTour { get; set; } = null!;
        [Column("FECHA")]
        public DateOnly FechaTour { get; set; }
        [Column("HORA")]
        public TimeOnly HoraTour { get; set; }

        [Required(ErrorMessage = "El precio del tour es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio del tour debe ser mayor que cero.")]
        [Column("PRECIO")]
        public decimal PrecioTour { get; set; }
        [Column("IMPUESTO")]
        public decimal ImpuestoTour => PrecioTour * 0.18m; // 18% de impuesto
        public decimal PrecioTotalTour => PrecioTour + ImpuestoTour; // Precio total con impuesto
        [Column("DURACIONHORAS")]
        public int DuracionTour { get; set; } // Duración en horas
        public DateTime FechaHoraFin => FechaTour.ToDateTime(HoraTour).AddHours(DuracionTour); // Fecha y hora de finalización del tour

        [Column("ESTADO")]
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

        [ForeignKey("idPais")]
        [JsonIgnore]
        public PaisModel? Pais { get; set; } = null!;
        [ForeignKey("idDestino")]
        [JsonIgnore]
        public DestinoModel? Destino { get; set; } = null!;
        [ForeignKey("idCategoria")]
        [JsonIgnore]
        public CategoriaModel? Categoria { get; set; } = null!;
        [ForeignKey("idGuiaTuristico")]
        [JsonIgnore]
        public GuiaTuristicoModel? GuiaTuristico { get; set; } = null!;
        [ForeignKey("idTransporte")]
        [JsonIgnore]
        public TransporteModel? Transporte { get; set; } = null!;
        //jsonignore para que no se muestre en el body cada vez que vaya a hacer un HTTPPOST
        //las llaves foraneas para cuando use el GET me devuelva los nombres de los campos que estoy utilizando

    }
}
