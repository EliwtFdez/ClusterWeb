using System.ComponentModel.DataAnnotations;

namespace ClusterWeb.DTOs
{
    public class CasaUpdateDto
    {
        [Required, MaxLength(200)]
        public string Direccion { get; set; }

        [Required, MaxLength(10)]
        public string NumeroCasa { get; set; }

        [Range(1, 50)]
        public int Habitaciones { get; set; }

        [Range(1, 20)]
        public int Banos { get; set; }
    }
}
