namespace Proyecto.Models
{
    public class NodoFecha
    {
        public DateTime feca;

        public List<Paciente> Agendacion;

        public NodoFecha(DateTime Fech, Paciente p2)
        {
            feca = Fech;
            Agendacion = new List<Paciente>();
            Agendacion.Add(p2);
        }
    }
}
