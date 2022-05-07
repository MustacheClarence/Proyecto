using Proyecto.Models;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto.Controllers
{
    public class PacienteController : Controller
    {
        static AVL ArbolVL = new AVL();
        static List<Paciente> pacientesList;
        static List<NodoFecha> AgendaList = new List<NodoFecha>();
        List<Paciente> pacientesDeArchivo;
        DateTime hoy = DateTime.Now;
        public IActionResult Index()
        {
            return View();
        }

        //....................................AGREGAR Y BUSCAR...............................
        public IActionResult Agregar(string nombre, string id, string edad, string tel, DateTime ultConsulta, DateTime proxConsulta, string diagnostico)
        {
            try
            {
                //..............................................................CODIGO PARA AGREGAR PACIENTES...........................................................
                if(nombre == null || id == null || edad == null || tel == null)
                {
                    return Content("Profavor ingrese todos los datos requeridos");
                }
                if(ArbolVL.YaEsta(id, ArbolVL.raiz)){
                    return Content("Ese ID ya esta ingresado, porfavor verifique sus datos.");
                }
                else
                {
                    Paciente paciente = new Paciente();
                    paciente.Name = nombre;
                    try
                    {
                        Convert.ToInt32(id);
                        paciente.Id = id;
                    }
                    catch (Exception e)
                    {
                        return Content("El ID debe ser un valor numerico.\n" + e.Message);
                    }
                    try
                    {
                        paciente.Age = Convert.ToInt32(edad);
                    }
                    catch (Exception e)
                    {
                        return Content("La edad debe ser numerica.\n" + e.Message);
                    }
                    try
                    {
                        Convert.ToInt32(tel);
                        paciente.Tel = tel;
                    }
                    catch (Exception e)
                    {
                        return Content("El telefono debe ser un valor numerico.\n" + e.Message);
                    }
                    paciente.LastConsult = ultConsulta;
                    paciente.ProxConsult = proxConsulta;
                    paciente.Diagnostico = diagnostico;

                    ArbolVL.Insertar(paciente);

                    return Content("1");
                }
                
            }
            catch (Exception e)
            {
                return Content("No se pudo agregar correctamente" + e.Message);
            }
            
        }
        public IActionResult Buscar(string nombre, string id)
        {// .......................PREGUNTAR SOBRE RETURN...................
            try
            {
                if (nombre == null && id == null)
                {
                    return Content("Ingrese el nombre o el dpi de la persona que quiera buscar");
                }
                if (id != null)
                {
                    try
                    {
                        Paciente paciente = ArbolVL.BuscarID(id, ArbolVL.raiz).paciente;
                        return View(paciente);
                    }
                    catch (Exception e)
                    {

                        return Content("El dpi no se encuentra en el sistema." + e);
                    }                    
                }
                else
                {
                    Paciente paciente = ArbolVL.BuscarNombre(nombre: nombre, r: ArbolVL.raiz, null);
                    if (paciente == null)
                    {
                        return View(new Paciente{Name = "No existe."});
                    }
                    else 
                    return View(paciente);
                }
            }
            catch (Exception e)
            {
                return Content("Oh oh, hubo un error inesperado , intente en otros momentos\n" + e.Message);
            }
        }

        //...................................SEGUIMIENTO DE TRATAMIENTOS.................................
        public IActionResult DRLD()
        {
            pacientesList = new List<Paciente>();
            //................OBTENER PACIENTES QUE DEBERIAN REALIZAR LIMPIEZA DENTAL...............................................
            pacientesList = ArbolVL.toList(pacientesList, ArbolVL.raiz);
            List<Paciente> LD = new List<Paciente>();
            if(pacientesList != null)
            {
                foreach (var paciente in pacientesList)
                {
                    if (paciente.Diagnostico == null && (hoy - paciente.LastConsult).TotalDays >= 180)//no tiene diagnostico y no ah ido en 6 meses o 180 dias
                    {
                        LD.Add(paciente);
                    }
                }

                return View(LD);
            }
            else
            {
                return Content("No hay pacientes registrados");
            }
            
        }
        public IActionResult DSTO()
        {
            pacientesList = new List<Paciente>();
            pacientesList = ArbolVL.toList(pacientesList, ArbolVL.raiz);
            //................OBTENER PACIENTES QUE DEBERIAN DAR SEGUIMIENTO DE SU TRATAMIENTO DE ORTODONCIA..........................
            List<Paciente> TO = new List<Paciente>();
           
            if (pacientesList != null)
            {
                foreach (var paciente in pacientesList)
                {
                    if(paciente.Diagnostico != null)
                    {
                        if ((paciente.Diagnostico.Contains("ortodoncia") || paciente.Diagnostico.Contains("Ortodoncia")) && ((hoy - paciente.LastConsult).TotalDays >= 60)) // es ortodonica y no a ido en 2 meses o 60 dias
                        {
                            TO.Add(paciente);
                        }
                    }                    
                }
                return View(TO);
            }
            else
            {
                return Content("No hay pacientes registrados");
            }
        }        
        public IActionResult DSTC()
        {
            pacientesList = new List<Paciente>();
            pacientesList = ArbolVL.toList(pacientesList, ArbolVL.raiz);
            //................OBTENER PACIENTES QUE DEBERIAN DAR SEGUIMIENTO DE SU TRATAMIENTO DE CARIES..........................
            List<Paciente> TC = new List<Paciente>();
            
            if (pacientesList != null)
            {
                foreach (var paciente in pacientesList)
                {
                    if(paciente.Diagnostico != null)
                    {
                        if ((paciente.Diagnostico.Contains("caries") || paciente.Diagnostico.Contains("Caries")) && ((hoy - paciente.LastConsult).TotalDays >= 120)) // es caries y no a ido en 4 meses o 120 dias
                        {
                            TC.Add(paciente);
                        }
                    }                    
                }
                return View(TC);
            }
            else
            {
                return Content("No hay pacientes registrados");
            }
        }
        public IActionResult DSTE()
        {
            pacientesList = new List<Paciente>();
            pacientesList = ArbolVL.toList(pacientesList, ArbolVL.raiz);
            //................OBTENER PACIENTES QUE DEBERIAN DAR SEGUIMIENTO DE SU TRATAMIENTO ESPECIFICO..........................
            bool ContainsOrto;
            bool ContainsCaries;
            bool Limpieza;

            List<Paciente> TE = new List<Paciente>();

            
            if(pacientesList != null)
            {
                foreach(var paciente in pacientesList)
                {
                    if (paciente.Diagnostico == null)
                    {
                        Limpieza = true;                       
                    }
                    else Limpieza = false;
                    if(paciente.Diagnostico != null)
                    {
                        if (paciente.Diagnostico.Contains("ortodoncia") || paciente.Diagnostico.Contains("Ortodoncia"))
                        {
                            ContainsOrto = true;
                        }
                        else ContainsOrto = false;
                        if (paciente.Diagnostico.Contains("caries") || paciente.Diagnostico.Contains("Caries"))
                        {
                            ContainsCaries = true;
                        }
                        else ContainsCaries = false;
                    }
                    else
                    {
                        ContainsOrto = false;
                        ContainsCaries = false;
                    }
                        



                    if (!ContainsCaries && !ContainsOrto && !Limpieza)// tiene diagnostico pero no es ni caries ni ortodoncia
                    {
                        TE.Add(paciente);
                    }
                }
                return View(TE);
            }
            else
            {
                return Content("No hay pacientes registrados");
            }
        }

        //.....................................REGISTRAR CONSULTA.................................
  
        bool Ready(DateTime Fech)
        {
            foreach (var fecaa in AgendaList)
            {                
                if (fecaa.feca == Fech)
                {
                    return true;
                }
            }
            return false;
        }
        public IActionResult LaAgg(DateTime FechaAgg, Paciente pp)
        {

            try
            {

                if (Ready(FechaAgg))
                {
                    foreach (var fecaa in AgendaList)
                    {

                        if (fecaa.feca == FechaAgg)
                        {
                            if (fecaa.Agendacion.Count() < 8)
                            {
                                fecaa.Agendacion.Add(pp);
                                ArbolVL.Editar(pp, FechaAgg);
                            }
                            else
                            {
                                return Content("Esta socado, porfavor ingrese otra fecha.");


                            }
                        }

                    }
                }
                else
                {

                    AgendaList.Add(new NodoFecha(FechaAgg, pp));
                    ArbolVL.Editar(pp, FechaAgg);
                }
                return Content("Te agendaste krnal");

            }
            catch (Exception e)
            {
                return Content("Perdon, algo anda mal, probablemente no hayan pacientes ingresados");
            }

        }
        public IActionResult Asignacion(string nombre, string id, DateTime proxConsulta)
        {
            try
            {
                if (nombre == null && id == null)
                {
                    return Content("Por favor ingrese nombre y Id");
                }
                try
                {
                    Convert.ToInt32(id);
                }
                catch (Exception e)
                {
                    return Content("ERROR 404: valor numerico not found.");
                }
                if (id != null)
                {

                    return LaAgg(proxConsulta, ArbolVL.BuscarID(id, ArbolVL.raiz).paciente);

                }
                else
                {
                    return LaAgg(proxConsulta, ArbolVL.BuscarID(nombre, ArbolVL.raiz).paciente);
                }

            }
            catch (Exception e)
            {
                return Content("Algo anda mal, probablemente no hayan pacientes ingresados");
            }
        }

        public IActionResult Editar(string id, string diagnostico)
        {
            try
            {
                ArbolVL.EditarD(ArbolVL.BuscarID(id, ArbolVL.raiz).paciente, diagnostico);
                return Content("Diagnostico editado correctamente");
            }
            catch (Exception e)
            {
                return Content("Oh oh, algo a salido mal");
            }
            
        }
        public IActionResult EditarDiagnostico()
        {
            return View(); 
        }


        //..................LEER ARCHIVO DE PRUEBA.....................................
        public IActionResult LeerArchivo(IFormFile file)
        {
            if (file != null)
            {
                pacientesDeArchivo = new List<Paciente>();
                try
                {
                    string acceso = Path.Combine(Path.GetTempPath(), file.Name);
                    using (var stream = new FileStream(acceso, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }


                    string Todo = System.IO.File.ReadAllText(acceso);
                    foreach (string Actual in Todo.Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(Actual))
                        {
                            string[] data = Actual.Split(',');
                            pacientesDeArchivo.Add(new Paciente()
                            {
                                Name = data[0],
                                Id = data[1],
                                Age = Convert.ToInt32(data[2]),
                                Tel = data[3],
                                LastConsult = Convert.ToDateTime(data[4]),
                                ProxConsult = Convert.ToDateTime(data[5]),
                                Diagnostico = data[6]
                            });
                            ArbolVL.Insertar(new Paciente()
                            {
                                Name = data[0],
                                Id = data[1],
                                Age = Convert.ToInt32(data[2]),
                                Tel = data[3],
                                LastConsult = Convert.ToDateTime(data[4]),
                                ProxConsult = Convert.ToDateTime(data[5]),
                                Diagnostico = data[6]
                            });
                        }
                    }

                }
                catch (Exception e)
                {
                    ViewBag.Error = e.Message;
                }
            }

            return View(pacientesDeArchivo);
        }
        public IActionResult AgregarMasivo(List<Paciente> lp)
        {
            foreach (var paciente in lp)
            {
                ArbolVL.Insertar(paciente);
            }
            return Content("Agregado!");
        }
    }

}
