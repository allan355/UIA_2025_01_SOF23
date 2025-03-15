using AutoMapper;
using CRUDGenerator.Contracts;
using CRUDGenerator.Interfaces;
using CRUDGenerator.Models;
using CRUDGenerator.AppDataContext;

namespace CRUDGenerator.Services
{
    public class PersonaServices : IPersonaServices
    {
        private readonly Context _context;
        private readonly ILogger<PersonaServices> _logger;
        private readonly IMapper _mapper;
        public PersonaServices(Context context, ILogger<PersonaServices> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task CreateAsync(ContractBase request)
        {
            try
            {
                var Persona = _mapper.Map<Persona>(request);
                _context.Persona.Add(Persona);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurri� un error al crear el elemento Persona.");
                throw new Exception("Ocurri� un error al crear el elemento Persona.");
            }
        }       

        public IEnumerable<ModelBase> GetAll()
        {
            var Persona = _context.Persona.ToList();
            if (Persona == null)
            {
                throw new Exception("No se encontraron elementos Persona");
            }
            return Persona;
        }

        public async Task<ModelBase> GetByIdAsync(Guid id)
        {
            var Persona = await _context.Persona.FindAsync(id);
            if (Persona == null)
            {
                throw new KeyNotFoundException($"No se encontr� ning�n elemento Sapmple con Id {id}.");
            }
            return Persona;
        }

        public async Task UpdateAsync(Guid id, ContractBase req)
        {
            try
            {
                var request = (PersonaRequest)req;
                var Persona = await _context.Persona.FindAsync(id);
                if (Persona == null)
                {
                    throw new Exception($"No se encontr� el elemento Persona con id {id}.");
                }

                if (request.id != null)
{
    Persona.id = request.id;
}
if (request.Cedula != null)
{
    Persona.Cedula = request.Cedula;
}
if (request.Nombre != null)
{
    Persona.Nombre = request.Nombre;
}
if (request.Apellido != null)
{
    Persona.Apellido = request.Apellido;
}


                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ocurri� un error al actualizar el elemento Persona con id {id}.");
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var Persona = await _context.Persona.FindAsync(id);
            if (Persona != null)
            {
                _context.Persona.Remove(Persona);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"No se encontr� ning�n elemento con el id {id}");
            }
        }

    }
}
