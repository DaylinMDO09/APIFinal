using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APITours.Modelos
{
    public class MetodoPagoModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdMetodoPago { get; set; }
        [Required(ErrorMessage = "El nombre del método de pago es obligatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre del método de pago debe tener entre 3 y 50 caracteres.")]
        [Column("NOMBRE")]
        public string NombreMetodoPago { get; set; } = null!;
    }
}
