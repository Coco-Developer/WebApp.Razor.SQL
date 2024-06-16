
using Microsoft.EntityFrameworkCore;

namespace AppLazaro.Models;

public partial class CrudDbContext : DbContext
{
    public CrudDbContext()
    {
    }

    public CrudDbContext(DbContextOptions<CrudDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Items> Items { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=CRUD_DB;Integrated Security=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Items>(entity =>
        {
            entity.HasKey(e => e.ID); // Define la clave primaria usando la propiedad ID de la entidad Items

            entity.Property(e => e.ID)
                .ValueGeneratedOnAdd()  // Indica que el valor de ID se genera automáticamente al agregar una nueva entidad
                .HasColumnName("ID");  // Nombre de la columna en la base de datos
        });

        OnModelCreatingPartial(modelBuilder);  // Llamada al método parcial para completar la configuración del modelo
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
