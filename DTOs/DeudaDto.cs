namespace ClusterWeb.DTOs
{
    public class DeudaDto
    {
        public int DeudaId { get; set; }
        public int ResidenteId { get; set; }
        public int CasaId { get; set; }
        public decimal Monto { get; set; }
        public decimal SaldoPendiente { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string Estado { get; set; }  // Se cambia de enum a string
        public string Descripcion { get; set; }
        public DateTime FechaRegistro { get; set; }
    }

    public class DeudaCreateDto
    {
        public int ResidenteId { get; set; }
        public int CasaId { get; set; }
        public decimal Monto { get; set; }
        public decimal SaldoPendiente { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string Estado { get; set; } = "pendiente";  // Valor por defecto acorde a la BD
        public string Descripcion { get; set; }
    }

    public class DeudaUpdateDto
    {
        public int ResidenteId { get; set; }
        public int CasaId { get; set; }
        public decimal Monto { get; set; }
        public decimal SaldoPendiente { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string Estado { get; set; }
        public string Descripcion { get; set; }
    }
}
