using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClusterWeb.Entities
{
    public class Deuda
    {
        [Key]
        public int DeudaId { get; set; }
        
        // Relación con Residente y Casa
        public int ResidenteId { get; set; }
        public int CasaId { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [Range(0.01, double.MaxValue)]
        public decimal Monto { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [Range(0, double.MaxValue)]
        public decimal SaldoPendiente { get; set; }

        public DateTime FechaVencimiento { get; set; }
        public EstadoDeuda Estado { get; set; } = EstadoDeuda.Pendiente;

        [MaxLength(500)]
        public string Descripcion { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // Navegación
        public virtual Residente Residente { get; set; }
        public virtual Casa Casa { get; set; }
    }

}
