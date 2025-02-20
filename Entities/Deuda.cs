using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClusterWeb.Entities
{
    public class Deuda
    {
        public int DeudaId { get; set; }
        public int ResidenteId { get; set; }
        public int CasaId { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Monto { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal SaldoPendiente { get; set; }

        public DateTime FechaVencimiento { get; set; }
        public string Estado { get; set; } = "pendiente";
        public string Descripcion { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public Residente Residente { get; set; }
        public Casa Casa { get; set; }
    }
}
