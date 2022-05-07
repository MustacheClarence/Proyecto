using Proyecto.Models;
namespace Proyecto.Models
{
    public class Nodo<T>
    {
        public T paciente;
        public Nodo<T> subIzq;
        public Nodo<T> subDer;
        public int FactorEquilibrio;

        public Nodo(T p)
        {
            this.paciente = p;
            this.subIzq = null;
            this.subDer = null;
            this.FactorEquilibrio = 0;
        }
    }
}
