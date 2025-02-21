using System;
using System.ComponentModel.DataAnnotations;

namespace ClusterWeb.DTOs
{
    public class ResidenteCreateDto
    {
        public int ResidenteId { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; }

        [Required, MaxLength(15)]
        public string Telefono { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public DateTime FechaIngreso { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int CasaId { get; set; } 

    }
}
