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
    }
}
