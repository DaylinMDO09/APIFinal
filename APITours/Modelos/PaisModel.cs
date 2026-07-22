using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace APITours.Modelos
{
    public class PaisModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPais { get; set; } = 0;
        [Required]
        public string NombrePais { get; set; } = string.Empty;
    }
}
