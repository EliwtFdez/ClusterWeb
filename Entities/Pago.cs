using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClusterWeb.Entities
{
    public class Pago
    {
        [Key]
        public int PagoId { get; set; }
        
        // Relación con Deuda (con la cual se puede acceder al Residente)
        public int DeudaId { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [Range(0.01, double.MaxValue)]
        public decimal MontoPagado { get; set; }

        public DateTime FechaPago { get; set; } = DateTime.Now;

        [Required]
        public MetodoPago MetodoPago { get; set; }

        // Navegación
        public virtual Deuda Deuda { get; set; }
    }
}
