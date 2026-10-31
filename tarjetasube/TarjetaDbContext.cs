using Microsoft.EntityFrameworkCore;

public class TarjetaDbContext : DbContext
{
    public TarjetaDbContext() { }

    public TarjetaDbContext(DbContextOptions<TarjetaDbContext> opciones) : base(opciones) { }

    public DbSet<Tarjeta> Tarjetas => Set<Tarjeta>();
    public DbSet<TarjetaTipo> TarjetaTipos => Set<TarjetaTipo>();
    public DbSet<Colectivo> Colectivos => Set<Colectivo>();
    public DbSet<ColectivoInterUrbano> ColectivosInterUrbanos => Set<ColectivoInterUrbano>();
    public DbSet<Boleto> Boletos => Set<Boleto>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=tarjeta.db");
        }
    }
}
