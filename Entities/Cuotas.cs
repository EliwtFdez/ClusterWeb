using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClusterWeb.Entities
{
    public class Cuota
    {
        [Key]
        public int IdCuota { get; set; }

        [Required]
        public string NombreCuota { get; set; }

        [Required]
        public decimal Monto { get; set; }

        public DateTime? FechaVencimiento { get; set; }

        public string Descripcion { get; set; }

        // EstadoDeuda es un enum: Pendiente, Pagada o Vencida
        public EstadoDeuda Estado { get; set; } = EstadoDeuda.Pendiente;

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // FK hacia Casa: una Cuota está asociada a UNA Casa
        [Required]
        public int IdCasa { get; set; }
        public virtual Casa Casa { get; set; }

        // FK hacia Residente: una Cuota está asociada a UN Residente
        [Required]
        public int IdResidente { get; set; }
        public virtual Residente Residente { get; set; }
    }
}
