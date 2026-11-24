using Microsoft.EntityFrameworkCore;
using ConsultasPlan.Models;

namespace ConsultasPlan.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Afiliado> Afiliados { get; set; }
    public DbSet<Consulta> Consultas { get; set; }
    public DbSet<Profesional> Profesionales { get; set; }
    public DbSet<Sede> Sedes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Consulta>()
            .Property(c => c.Estado)
            .HasConversion<string>();

        modelBuilder.Entity<Consulta>()
            .Property(c => c.Modalidad)
            .HasConversion<string>();

        modelBuilder.Entity<Consulta>()
            .HasOne(c => c.Afiliado)
            .WithMany()
            .HasForeignKey(c => c.AfiliadoId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
