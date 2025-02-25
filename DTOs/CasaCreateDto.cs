using System.ComponentModel.DataAnnotations;

namespace ClusterWeb.DTOs
{
    public class CasaCreateDto
    {
        [Required]
        public int IdCasa { get; set; }

        [Required, MaxLength(10)]
        public string NumeroCasa { get; set; }

        [MaxLength(200)]
        public string? Direccion { get; set; }

        public List<ResidenteCreateDto> Residentes { get; set; } = new List<ResidenteCreateDto>();

        public List<CuotaCreateDto> Cuotas { get; set; } = new List<CuotaCreateDto>();
    }
}
