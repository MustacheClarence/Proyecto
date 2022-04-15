using Proyecto.Models;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto.Controllers
{
    public class PacienteController : Controller
    {
        static AVL ArbolVL = new AVL();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Agregar(string nombre, string id, string edad, string tel, DateTime ultConsulta, DateTime proxConsulta, string diagnostico)
        {
            try
            {
                //AGREGAR CODIGO PARA AGREGAR PACIENTES !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                if(nombre == null || id == null || edad == null || tel == null)
                {
                    return Content("Profavor ingrese todos los datos requeridos");
                }
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
            catch (Exception e)
            {
                return Content("No se pudo agregar correctamente" + e.Message);
            }
            
        }
    }
}
