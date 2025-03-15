using System.ComponentModel.DataAnnotations;

namespace CRUDGenerator.Contracts
{
    public class PersonaRequest : ContractBase
    {
        public Guid id { get; set; }
public string Cedula { get; set; }
public string Nombre { get; set; }
public string Apellido { get; set; }


        public PersonaRequest() {}
    }
}