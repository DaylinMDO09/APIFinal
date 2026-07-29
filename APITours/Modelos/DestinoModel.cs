using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APITours.Modelos
{
    public class DestinoModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdDestino { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre del destino debe tener entre 3 y 100 caracteres.")]
        public string NombreDestino { get; set; } = null!;
        [ForeignKey("Pais")]
        public int IdPais { get; set; }
    }
}
