using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace APITours.Modelos
{
    public class GuiaTuristicoModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdGuiaTuristico { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre del guía turístico debe tener entre 3 y 100 caracteres.")]
        [Column("NOMBRE")]
        public string NombreGuiaTuristico { get; set; } = null!;
        [Required]
        [StringLength(15, MinimumLength = 7, ErrorMessage = "El teléfono del guía turístico debe tener entre 7 y 15 caracteres.")]
        [Column("TELEFONO")]
        public string TelefonoGuiaTuristico { get; set; } = null!;
        [Required]
        [EmailAddress(ErrorMessage = "El correo electrónico del guía turístico no es válido.")]
        [Column("CORREOGUIA")]
        public string CorreoGuiaTuristico { get; set; } = null!;
    }
}
