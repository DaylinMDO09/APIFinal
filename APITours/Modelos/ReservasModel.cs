using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace APITours.Modelos
{
    public class ReservasModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdReserva { get; set; }
        public DateTime FechaReserva { get; set; } = DateTime.Now;

        public int idCliente { get; set; } = 0;
        public int idTour { get; set; } = 0;
        public int idMetodoPago { get; set; } = 0;

        [ForeignKey("idCliente")]
        [JsonIgnore]
        public ClientesModel? Cliente { get; set; } = null!;
        [ForeignKey("idTour")]
        [JsonIgnore]
        public ToursModel? Tour { get; set; } = null!;
        [ForeignKey("idMetodoPago")]
        [JsonIgnore]
        public MetodoPagoModel? MetodoPago { get; set; } = null!;
    }
}
