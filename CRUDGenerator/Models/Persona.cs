using System.ComponentModel.DataAnnotations;

namespace CRUDGenerator.Models
{
    public class Persona : ModelBase
    {
        public Guid id { get; set; }
public string Cedula { get; set; }
public string Nombre { get; set; }
public string Apellido { get; set; }


        public Persona()
        {
        }
    }
}