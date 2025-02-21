using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ClusterWeb.Entities;

namespace ClusterWeb.DTOs
{
    public class PagoDto
    {
        public int PagoId { get; set; }
        public int DeudaId { get; set; }
        public decimal MontoPagado { get; set; }
        public DateTime FechaPago { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MetodoPago MetodoPago { get; set; }
        public int? ResidenteId { get; set; }
    }

    public class PagoCreateDto
    {
        [Required]
        public int DeudaId { get; set; }
        
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto pagado debe ser mayor a cero.")]
        public decimal MontoPagado { get; set; }
        
        public DateTime FechaPago { get; set; } = DateTime.Now;
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required]
        public MetodoPago MetodoPago { get; set; }
        public int? ResidenteId { get; set; }
    }

    public class PagoUpdateDto
    {
        [Required]
        public int DeudaId { get; set; }
        
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto pagado debe ser mayor a cero.")]
        public decimal MontoPagado { get; set; }
        
        public DateTime FechaPago { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required]
        public MetodoPago MetodoPago { get; set; }
        public int? ResidenteId { get; set; }
    }
}
