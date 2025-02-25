using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClusterWeb.Entities
{
    public class Pago
    {
        [Key]
        public int IdPago { get; set; }

        [Required]
        public int IdCasa { get; set; }
        public virtual Casa Casa { get; set; }

        public int? IdResidente { get; set; }
        public virtual Residente Residente { get; set; }

        [Required]
        public int IdCuota { get; set; }
        public virtual Cuota Cuota { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0.01, double.MaxValue)]
        public decimal MontoPagado { get; set; }

        public DateTime FechaPago { get; set; } = DateTime.Now;

        [Required]
        public MetodoPago MetodoPago { get; set; }

        public string Observaciones { get; set; }
    }
}
