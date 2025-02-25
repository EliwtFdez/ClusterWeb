using System;
using System.ComponentModel.DataAnnotations;

namespace ClusterWeb.DTOs
{
    public class ResidenteCreateDto
    {
        public int IdResidente { get; set; }

        [Required, MaxLength(50)]
        public string Nombre { get; set; }

        [Phone]
        public string Telefono { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public int IdCasa { get; set; } 
    }
}
