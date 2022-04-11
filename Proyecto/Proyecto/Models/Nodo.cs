using Proyecto.Models;
namespace Proyecto.Models
{
    public class Nodo
    {
        public Paciente paciente;
        public Nodo subIzq;
        public Nodo subDer;
        public int FactorEquilibrio;

        public Nodo(Paciente p)
        {
            this.paciente = p;
            this.subIzq = null;
            this.subDer = null;
            this.FactorEquilibrio = 0;
        }
    }
}
