namespace Proyecto.Models
{
    public class Paciente
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public int Age { get; set; }
        public string Tel { get; set; }
        public DateTime LastConsult { get; set; }
        public DateTime ProxConsult { get; set; }
        public string Diagnostico { get; set; }
    }
}
