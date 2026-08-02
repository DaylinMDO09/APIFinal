using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APITours.Modelos
{
    public class ClientesModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCliente { get; set; }
        [Required(ErrorMessage = "El nombre completo del cliente es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre del cliente debe tener entre 3 y 100 caracteres.")]
        public string NombreCompleto { get; set; } = null!;
        [Required(ErrorMessage = "El correo electrónico del cliente es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string CorreoCliente { get; set; } = null!;
        [Required(ErrorMessage = "El número de teléfono del cliente es obligatorio.")]
        [Phone(ErrorMessage = "El número de teléfono no es válido.")]
        public string TelefonoCliente { get; set; } = null!;
    }
}
