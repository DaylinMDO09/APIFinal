using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    }
}
