using System;
using System.ComponentModel.DataAnnotations;
using ClusterWeb.Entities;

namespace ClusterWeb.DTOs
{
    // DTO para mostrar la información del pago
    public class PagoDto
    {
        public int PagoId { get; set; }
        public int DeudaId { get; set; }
        public int ResidenteId { get; set; }
        public decimal MontoPagado { get; set; }
        public DateTime FechaPago { get; set; }
        public MetodoPago MetodoPago { get; set; }
    }

    // DTO para la creación de un nuevo pago
    public class PagoCreateDto
    {
        [Required]
        public int DeudaId { get; set; }
        
        [Required]
        public int ResidenteId { get; set; }
        
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto pagado debe ser mayor a cero.")]
        public decimal MontoPagado { get; set; }
        
        public DateTime FechaPago { get; set; } = DateTime.Now;
        
        [Required]
        [RegularExpression("^(Efectivo|TarjetaCredito|Transferencia)$", 
            ErrorMessage = "El método de pago debe ser 'Efectivo', 'TarjetaCredito' o 'Transferencia'.")]
        public string MetodoPago { get; set; }
    }

    // DTO para actualizar un pago existente
    public class PagoUpdateDto
    {
        [Required]
        public int DeudaId { get; set; }
        
        [Required]
        public int ResidenteId { get; set; }
        
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto pagado debe ser mayor a cero.")]
        public decimal MontoPagado { get; set; }
        
        public DateTime FechaPago { get; set; }
        
        [Required]
        public MetodoPago MetodoPago { get; set; }
    }
}
