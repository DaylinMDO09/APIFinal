using System.ComponentModel.DataAnnotations;
namespace APITours.Modelos.Autenticacion
{
    public class LogInRespondido
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string ClaveUsuario { get; set; } = string.Empty;
    }
}
