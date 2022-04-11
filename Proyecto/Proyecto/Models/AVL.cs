namespace Proyecto.Models
{
    public class AVL
    {
        Nodo raiz;
        public AVL()
        {
            this.raiz = null;
        }

        //Busqueda
        public Nodo BuscarID(string id, Nodo r)
        {
            if (raiz == null)
            {
                return null;
            }
            else if (r.paciente.Id == id)
            {
                return r;
            }
            else if (string.Compare(r.paciente.Id, id) < 0)
            {
                return BuscarID(id, r.subDer);
            }
            else if (string.Compare(r.paciente.Id, id) > 0)
            {
                return BuscarID(id, r.subIzq);
            }
            else
            {
                return null;
            }

        }
        public Nodo BuscarNombre(string nombre, Nodo r)
        {
            if (raiz == null)
            {
                return null;
            }
            else if (r.paciente.Name == nombre)
            {
                return r;
            }
            else if (string.Compare(r.paciente.Name, nombre) < 0)
            {
                return BuscarID(nombre, r.subDer);
            }
            else if (string.Compare(r.paciente.Name, nombre) > 0)
            {
                return BuscarID(nombre, r.subIzq);
            }
            else
            {
                return null;
            }
        }

        //Obtener factor de equilibrio
        int FE(Nodo arbol)
        {
            if (arbol == null)
            {
                return -1;
            }
            else
            {
                return arbol.FactorEquilibrio;
            }
        }
        //Rotaciones simples
        Nodo rotIzq(Nodo arbol)
        {
            Nodo aux = arbol.subIzq;
            arbol.subIzq = aux.subDer;
            aux.subDer = arbol;
            arbol.FactorEquilibrio = Math.Max(FE(arbol.subIzq), FE(arbol.subDer)) + 1;
            aux.FactorEquilibrio = Math.Max(FE(aux.subIzq), FE(aux.subDer)) + 1;
            return aux;
        }
        Nodo rotDer(Nodo arbol)
        {
            Nodo aux = arbol.subDer;
            arbol.subDer = aux.subIzq;
            aux.subIzq = arbol;
            arbol.FactorEquilibrio = Math.Max(FE(arbol.subIzq), FE(arbol.subDer)) + 1;
            aux.FactorEquilibrio = Math.Max(FE(aux.subIzq), FE(aux.subDer)) + 1;
            return aux;
        }
        // Rotaciones dobles
        Nodo DrotIzq(Nodo arbol)
        {
            Nodo aux;
            arbol.subIzq = rotDer(arbol.subIzq);
            aux = rotIzq(arbol);
            return aux;
        }
        Nodo DrotDer(Nodo arbol)
        {
            Nodo aux;
            arbol.subDer = rotIzq(arbol.subDer);
            aux = rotDer(arbol);
            return aux;
        }

        //insertar segun el orden
        public void Insertar(Paciente nuevo)
        {
            Nodo nuevoArbol = new Nodo(nuevo);
            if (raiz == null)
            {
                raiz = nuevoArbol;
            }
            else
            {
                raiz = InsertarID(nuevoArbol, raiz);
            }
        }
        Nodo InsertarID(Nodo nuevo, Nodo subArbol)
        {
            //nuevo = nuevo dato a ingresar
            //subArbol = raiz actual del AVL
            //nuevoPadre = raiz del AVL despues de las rotaciones

            Nodo nuevoPadre = subArbol;
            // si es menor
            if (string.Compare(nuevo.paciente.Id, subArbol.paciente.Id) < 0)
            {
                // si la raiz no tiene hijo izquiedo, se asigna en esa posicion
                if (subArbol.subIzq == null)
                {
                    subArbol.subIzq = nuevo;
                }
                // si se tiene un hijo izquierdo se utiliza la recursion para revisar los hijos del subArbol izquierdo
                else
                {
                    subArbol.subIzq = InsertarID(nuevo, subArbol.subIzq);
                    //Si el arbol entra en un desbalance, se utilizan rotaciones
                    if ((FE(subArbol.subIzq) - FE(subArbol.subDer) == 2))
                    {
                        if (string.Compare(nuevo.paciente.Id, subArbol.subIzq.paciente.Id) < 0)
                        {
                            nuevoPadre = rotIzq(subArbol);
                        }
                        else
                        {
                            nuevoPadre = DrotIzq(subArbol);
                        }
                    }
                }
            }
            //si es mayor
            else if (string.Compare(nuevo.paciente.Id, subArbol.paciente.Id) >= 0)
            {
                // si la raiz no tiene hijo derecho, se asigna en esa posicion
                if (subArbol.subDer == null)
                {
                    subArbol.subDer = nuevo;
                }
                else
                {
                    // si se tiene un hijo derecho se utiliza la recursion para revisar los hijos del subArbol derecho
                    subArbol.subDer = InsertarID(nuevo, subArbol.subDer);
                    //Si el arbol entra en un desbalance, se utilizan rotaciones
                    if ((FE(subArbol.subDer) - FE(subArbol.subIzq) == 2))
                    {
                        if (string.Compare(nuevo.paciente.Id, subArbol.subDer.paciente.Id) >= 0)
                        {
                            nuevoPadre = rotDer(subArbol);
                        }
                        else
                        {
                            nuevoPadre = DrotDer(subArbol);
                        }
                    }
                }
            }
            if ((subArbol.subIzq == null) && (subArbol.subDer != null))
            {
                subArbol.FactorEquilibrio = subArbol.subDer.FactorEquilibrio + 1;
            }
            else if ((subArbol.subDer == null) && (subArbol.subIzq != null))
            {
                subArbol.FactorEquilibrio = subArbol.subIzq.FactorEquilibrio + 1;
            }
            else
            {
                subArbol.FactorEquilibrio = Math.Max(FE(subArbol.subIzq), FE(subArbol.subDer)) + 1;
            }
            return nuevoPadre;

        }
    }
}
