using Microsoft.EntityFrameworkCore;
namespace APITours.Modelos
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UsuariosModel> Usuarios { get; set; }
        public DbSet<PaisModel> Pais { get; set; }
        public DbSet<DestinoModel> Destino { get; set; }
        public DbSet<CategoriaModel> Categoria { get; set; }
        public DbSet<GuiaTuristicoModel> GuiaTuristico { get; set; }
        public DbSet<TransporteModel> Transporte { get; set; }
        public DbSet<ClientesModel> Clientes { get; set; }
        public DbSet<MetodoPagoModel> MetodoPago { get; set; }
    }
}
