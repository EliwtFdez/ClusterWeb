namespace ClusterWeb.Entities
{
    public class Casa
    {
        public int CasaId { get; set; }
        public string Direccion { get; set; }
        public string NumeroCasa { get; set; }
        public int Habitaciones { get; set; }
        public int Banos { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // Relación con Residentes
        public ICollection<Residente> Residentes { get; set; } = new List<Residente>();
        public ICollection<Deuda> Deudas { get; set; }  = new List<Deuda>();
    }


}
