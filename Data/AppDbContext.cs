using ClusterWeb.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClusterWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Casa> Casas { get; set; }
        public DbSet<Residente> Residentes { get; set; }
        public DbSet<Deuda> Deudas { get; set; }
        public DbSet<Pago> Pagos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Índices
            modelBuilder.Entity<Casa>()
                .HasIndex(c => c.Direccion)
                .HasDatabaseName("idx_direccion");

            modelBuilder.Entity<Residente>()
                .HasIndex(r => r.Nombre)
                .HasDatabaseName("idx_nombre");

            // Valores por defecto
            modelBuilder.Entity<Casa>()
                .Property(c => c.FechaRegistro)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Residente>()
                .Property(r => r.FechaRegistro)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Deuda>()
                .Property(d => d.FechaRegistro)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Deuda>()
                .Property(d => d.Estado)
                .HasDefaultValue("pendiente")
                .HasConversion<string>();

            // Relaciones y claves foráneas
            modelBuilder.Entity<Residente>()
                .HasOne(r => r.Casa)
                .WithMany(c => c.Residentes)
                .HasForeignKey(r => r.CasaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Deuda>()
                .HasOne(d => d.Casa)
                .WithMany(c => c.Deudas)
                .HasForeignKey(d => d.CasaId)
                .OnDelete(DeleteBehavior.NoAction); // Evita múltiples cascadas

            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Residente)
                .WithMany(r => r.Pagos)
                .HasForeignKey(p => p.ResidenteId)
                .OnDelete(DeleteBehavior.NoAction); // Evita ciclos de cascada
        }
    }
}
