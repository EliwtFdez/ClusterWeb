using ClusterWeb.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
            // 🔹 Índices
            modelBuilder.Entity<Casa>()
                .HasIndex(c => c.Direccion)
                .HasDatabaseName("idx_direccion");

            modelBuilder.Entity<Residente>()
                .HasIndex(r => r.Nombre)
                .HasDatabaseName("idx_nombre");

            // 🔹 Valores por defecto
            modelBuilder.Entity<Casa>()
                .Property(c => c.FechaRegistro)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Residente>()
                .Property(r => r.FechaRegistro)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Deuda>()
                .Property(d => d.FechaRegistro)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Pago>()
                .Property(p => p.FechaPago)
                .HasDefaultValueSql("GETDATE()");

            // 🔹 Conversión de enums
            modelBuilder.Entity<Deuda>()
                .Property(d => d.Estado)
                .HasDefaultValue(EstadoDeuda.Pendiente)
                .HasConversion<string>();

            modelBuilder.Entity<Pago>()
                .Property(p => p.MetodoPago)
                .HasConversion<string>();

            // 🔹 Relaciones y claves foráneas
            modelBuilder.Entity<Residente>()
                .HasOne(r => r.Casa)
                .WithMany(c => c.Residentes)
                .HasForeignKey(r => r.CasaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Deuda>()
                .HasOne(d => d.Casa)
                .WithMany(c => c.Deudas)
                .HasForeignKey(d => d.CasaId)
                .OnDelete(DeleteBehavior.Restrict); // Evita cascada de eliminación

            modelBuilder.Entity<Deuda>()
                .HasOne(d => d.Residente)
                .WithMany(r => r.Deudas)
                .HasForeignKey(d => d.ResidenteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Deuda)
                .WithMany()
                .HasForeignKey(p => p.DeudaId)
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.DropColumn(nameof:"ResidenteId",Table: "Pagos");

        }
    }
}
