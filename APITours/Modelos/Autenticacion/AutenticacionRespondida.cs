namespace APITours.Modelos.Autenticacion
{
    public class AutenticacionRespondida
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiracion { get; set; } = DateTime.UtcNow.AddHours(1);
        public string NombreUsuario { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
    }
}
