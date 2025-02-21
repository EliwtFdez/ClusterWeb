using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClusterWeb.Entities
{
    public class Residente
    {
        [Key]
        public int ResidenteId { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; }

        [Required, Phone]
        public string Telefono { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public DateTime FechaIngreso { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // Relación con Casa
        public int CasaId { get; set; }
        public virtual Casa Casa { get; set; }

        // Relaciones
        public virtual ICollection<Deuda> Deudas { get; set; } = new List<Deuda>();
        public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    }
}
