using System.ComponentModel.DataAnnotations;

namespace ClusterWeb.Entities
{
    public class Casa
    {
        [Key]
        public int IdCasa { get; set; }
        
        [Required, MaxLength(10)]
        public string NumeroCasa { get; set; }
        
        [MaxLength(200)]
        public string? Direccion { get; set; }

        // Relación "Casa -> muchos Residentes"
        public virtual ICollection<Residente> Residentes { get; set; } = new List<Residente>();

        // Relación "Casa -> muchas Cuotas"
        public virtual ICollection<Cuota> Cuotas { get; set; } = new List<Cuota>();
    }
}