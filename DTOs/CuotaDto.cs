using ClusterWeb.Entities;

namespace ClusterWeb.DTOs
{
    public class CuotaDto
    {
        public int IdCuota { get; set; }
        public string NombreCuota { get; set; }
        public decimal Monto { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }  // Asumiendo que EstadoDeuda se convierte a string
        public DateTime FechaRegistro { get; set; }
        public int IdCasa { get; set; }
        public int IdResidente { get; set; }
        public List<ResidenteCreateDto> Residentes { get; set; } = new List<ResidenteCreateDto>();
    }

    public class CuotaCreateDto
    {
        public string NombreCuota { get; set; }
        public decimal Monto { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; } = "Pendiente"; // Valor por defecto acorde a la entidad
        public int IdCasa { get; set; }
        public int IdResidente { get; set; }

    }

    public class CuotaUpdateDto
    {
        public string NombreCuota { get; set; }
        public decimal Monto { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public int IdCasa { get; set; }
        public int IdResidente { get; set; }
    }
}
