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
    }
}
