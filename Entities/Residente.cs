using System;
using System.Collections.Generic;

namespace ClusterWeb.Entities
{
    public class Residente
    {
        public int ResidenteId { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // Clave foránea
        public int CasaId { get; set; }
        public Casa Casa { get; set; }

        public ICollection<Deuda> Deudas { get; set; }
        public ICollection<Pago> Pagos { get; set; }
    }
}
