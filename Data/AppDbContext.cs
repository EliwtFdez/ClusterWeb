using ClusterWeb.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClusterWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets = tablas en la base de datos
        public DbSet<Casa> Casas { get; set; }
        public DbSet<Residente> Residentes { get; set; }
        public DbSet<Cuota> Cuotas { get; set; }
        public DbSet<Pago> Pagos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // =========================================
            // 1) ÍNDICES
            // =========================================
            modelBuilder.Entity<Casa>()
                .HasIndex(c => c.NumeroCasa)
                .HasDatabaseName("idx_numero_casa")
                .IsUnique();

            modelBuilder.Entity<Residente>()
                .HasIndex(r => r.Email)
                .HasDatabaseName("idx_email")
                .IsUnique();

            // =========================================
            // 2) VALORES POR DEFECTO Y CONFIGURACIONES
            // =========================================
            modelBuilder.Entity<Cuota>()
                .Property(d => d.FechaRegistro)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Pago>()
                .Property(p => p.FechaPago)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Cuota>()
                .Property(c => c.Monto)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Pago>()
                .Property(p => p.MontoPagado)
                .HasColumnType("decimal(10,2)");

            // =========================================
            // 3) CONVERSIÓN DE ENUMS
            // =========================================
            modelBuilder.Entity<Cuota>()
                .Property(d => d.Estado)
                .HasDefaultValue(EstadoDeuda.Pendiente)
                .HasConversion<string>();

            modelBuilder.Entity<Pago>()
                .Property(p => p.MetodoPago)
                .HasConversion<string>();

            // =========================================
            // 4) RELACIONES Y CLAVES FORÁNEAS
            // =========================================
            // CASA -> RESIDENTE (1 a muchos)
            modelBuilder.Entity<Residente>()
                .HasOne(r => r.Casa)
                .WithMany(c => c.Residentes)
                .HasForeignKey(r => r.IdCasa)
                .OnDelete(DeleteBehavior.Restrict);

            // CASA -> CUOTA (1 a muchos)
            modelBuilder.Entity<Cuota>()
                .HasOne(cu => cu.Casa)
                .WithMany(c => c.Cuotas)
                .HasForeignKey(cu => cu.IdCasa)
                .OnDelete(DeleteBehavior.Restrict);

            // RESIDENTE -> CUOTA (1 a muchos)
            modelBuilder.Entity<Cuota>()
                .HasOne(cu => cu.Residente)
                .WithMany(r => r.Cuotas)
                .HasForeignKey(cu => cu.IdResidente)
                .OnDelete(DeleteBehavior.Restrict);

            // PAGO -> CUOTA (muchos a 1)
            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Cuota)
                .WithMany()
                .HasForeignKey(p => p.IdCuota)
                .OnDelete(DeleteBehavior.Restrict);

            // PAGO -> CASA (muchos a 1)
            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Casa)
                .WithMany()
                .HasForeignKey(p => p.IdCasa)
                .OnDelete(DeleteBehavior.Restrict);

            // PAGO -> RESIDENTE (muchos a 1, opcional)
            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Residente)
                .WithMany(r => r.Pagos)
                .HasForeignKey(p => p.IdResidente)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
