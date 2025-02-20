using System;

namespace ClusterWeb.Entities
{
    public class Pago
    {
        public int PagoId { get; set; }
        public int DeudaId { get; set; }
        public int ResidenteId { get; set; }
        public decimal MontoPagado { get; set; }
        public DateTime FechaPago { get; set; } = DateTime.Now;
        public string MetodoPago { get; set; }

        public Deuda Deuda { get; set; }
        public Residente? Residente { get; set; }
    }
}
