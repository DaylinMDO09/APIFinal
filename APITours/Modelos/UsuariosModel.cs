using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace APITours.Modelos
{
    public class UsuariosModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; } = 0;
        [Required]
        public string NombreCompleto { get; set; } = string.Empty;
        [Required]
        public string NombreUsuario { get; set; } = string.Empty;
        [Required]
        public string CorreoUsuario { get; set; } = string.Empty;
        [Required]
        public string ClaveUsuario { get; set; } = string.Empty;
    }
}
