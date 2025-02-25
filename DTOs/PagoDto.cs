using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ClusterWeb.Entities;

namespace ClusterWeb.DTOs
{
    public class PagoDto
    {
        public int IdPago { get; set; }
        public int IdCasa { get; set; }
        public int? IdResidente { get; set; }
        public int IdCuota { get; set; }
        public decimal MontoPagado { get; set; }
        public DateTime FechaPago { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MetodoPago MetodoPago { get; set; }
        public string Observaciones { get; set; }
    }

    public class PagoCreateDto
    {
        [Required]
        public int IdCasa { get; set; }
        
        public int? IdResidente { get; set; }
        
        [Required]
        public int IdCuota { get; set; }
        
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto pagado debe ser mayor a cero.")]
        public decimal MontoPagado { get; set; }
        
        public DateTime FechaPago { get; set; } = DateTime.Now;
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required]
        public MetodoPago MetodoPago { get; set; }
        
        public string Observaciones { get; set; }
    }

    public class PagoUpdateDto
    {
        [Required]
        public int IdCasa { get; set; }
        
        public int? IdResidente { get; set; }
        
        [Required]
        public int IdCuota { get; set; }
        
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto pagado debe ser mayor a cero.")]
        public decimal MontoPagado { get; set; }
        
        public DateTime FechaPago { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required]
        public MetodoPago MetodoPago { get; set; }
        
        public string Observaciones { get; set; }
        public List<ResidenteCreateDto> Residentes { get; set; } = new List<ResidenteCreateDto>();
    }
}
