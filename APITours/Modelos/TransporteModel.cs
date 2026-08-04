using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace APITours.Modelos
{
    public class TransporteModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTransporte { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El tipo de transporte debe tener entre 3 y 100 caracteres.")]
        public string TipoTransporte { get; set; } = null!;
        [Required]
        [Range(1, 100, ErrorMessage = "La capacidad de transporte debe ser entre 1 y 100.")]
        [Column("CAPACIDAD")]
        public int CapacidadTransporte { get; set; }
    }
}
