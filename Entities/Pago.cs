using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClusterWeb.Entities
{
    public class Pago
    {
        [Key]
        public int PagoId { get; set; }
        public int DeudaId { get; set; }
        public int ResidenteId { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [Range(0.01, double.MaxValue)]
        public decimal MontoPagado { get; set; }

        public DateTime FechaPago { get; set; } = DateTime.Now;
        public MetodoPago MetodoPago { get; set; }

        public virtual Deuda Deuda { get; set; }
        public virtual Residente Residente { get; set; }
    }
}
