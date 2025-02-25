using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClusterWeb.Entities
{
    public class Residente
    {
        [Key]
        public int IdResidente { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }

        [Phone]
        public string Telefono { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        // Relación con Casa
        [Required]
        public int IdCasa { get; set; }
        public virtual Casa Casa { get; set; }

        // Relaciones
        public virtual ICollection<Cuota> Cuotas { get; set; } = new List<Cuota>();
        public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    }
}