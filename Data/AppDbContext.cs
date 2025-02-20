using ClusterWeb.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
            // Índice en la dirección de la casa
            modelBuilder.Entity<Casa>()
                .HasIndex(c => c.Direccion)
                .HasDatabaseName("idx_direccion"); // Usar HasDatabaseName para definir el nombre del índice en la base de datos

            // Índice en el nombre del residente
            modelBuilder.Entity<Residente>()
                .HasIndex(r => r.Nombre)
                .HasDatabaseName("idx_nombre"); // Usar HasDatabaseName para definir el nombre del índice en la base de datos

            // Valor por defecto en FechaRegistro
            modelBuilder.Entity<Casa>()
                .Property(c => c.FechaRegistro)
                .HasDefaultValueSql("GETDATE()"); // Usar HasDefaultValueSql para definir un valor por defecto en SQL

            modelBuilder.Entity<Residente>()
                .Property(r => r.FechaRegistro)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Deuda>()
                .Property(d => d.FechaRegistro)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Pago>()
                .Property(p => p.FechaPago)
                .HasDefaultValueSql("GETDATE()");

            // Restricción de estado en la deuda
            modelBuilder.Entity<Deuda>()
                .Property(d => d.Estado)
                .HasDefaultValue("pendiente") // Usar HasDefaultValue para definir un valor por defecto
                .HasConversion<string>(); // Convertir el valor a string en la base de datos

            // Restricción de método de pago en el pago
            modelBuilder.Entity<Pago>()
                .Property(p => p.MetodoPago)
                .HasConversion<string>(); // Convertir el valor a string en la base de datos

            // Relaciones y claves foráneas
            modelBuilder.Entity<Residente>()
                .HasOne(r => r.Casa)
                .WithMany(c => c.Residentes)
                .HasForeignKey(r => r.CasaId)
                .OnDelete(DeleteBehavior.Cascade); // Eliminación en cascada

            modelBuilder.Entity<Deuda>()
                .HasOne(d => d.Residente)
                .WithMany(r => r.Deudas)
                .HasForeignKey(d => d.ResidenteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Deuda>()
                .HasOne(d => d.Casa)
                .WithMany(c => c.Deudas)
                .HasForeignKey(d => d.CasaId)
                .OnDelete(DeleteBehavior.NoAction); // No acción en eliminación

            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Deuda)
                .WithMany()
                .HasForeignKey(p => p.DeudaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Residente)
                .WithMany(r => r.Pagos)
                .HasForeignKey(p => p.ResidenteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}