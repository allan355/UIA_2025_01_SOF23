using AutoMapper;
using CRUDGenerator.Contracts;
using CRUDGenerator.Interfaces;
using CRUDGenerator.Models;
using CRUDGenerator.AppDataContext;

namespace CRUDGenerator.Services
{
    public class AllanTestServices : IAllanTestServices
    {
        private readonly Context _context;
        private readonly ILogger<AllanTestServices> _logger;
        private readonly IMapper _mapper;
        public AllanTestServices(Context context, ILogger<AllanTestServices> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task CreateAsync(ContractBase request)
        {
            try
            {
                var AllanTest = _mapper.Map<AllanTest>(request);
                _context.AllanTest.Add(AllanTest);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurri� un error al crear el elemento AllanTest.");
                throw new Exception("Ocurri� un error al crear el elemento AllanTest.");
            }
        }       

        public IEnumerable<ModelBase> GetAll()
        {
            var AllanTest = _context.AllanTest.ToList();
            if (AllanTest == null)
            {
                throw new Exception("No se encontraron elementos AllanTest");
            }
            return AllanTest;
        }

        public async Task<ModelBase> GetByIdAsync(Guid id)
        {
            var AllanTest = await _context.AllanTest.FindAsync(id);
            if (AllanTest == null)
            {
                throw new KeyNotFoundException($"No se encontr� ning�n elemento Sapmple con Id {id}.");
            }
            return AllanTest;
        }

        public async Task UpdateAsync(Guid id, ContractBase req)
        {
            try
            {
                var request = (AllanTestRequest)req;
                var AllanTest = await _context.AllanTest.FindAsync(id);
                if (AllanTest == null)
                {
                    throw new Exception($"No se encontr� el elemento AllanTest con id {id}.");
                }

                if (request.id != null)
{
    AllanTest.id = request.id;
}
if (request.idzbgjv != null)
{
    AllanTest.idzbgjv = request.idzbgjv;
}
if (request.gdfg != null)
{
    AllanTest.gdfg = request.gdfg;
}
if (request.erstgfdgh != null)
{
    AllanTest.erstgfdgh = request.erstgfdgh;
}
if (request.dfbfnh != null)
{
    AllanTest.dfbfnh = request.dfbfnh;
}


                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ocurri� un error al actualizar el elemento AllanTest con id {id}.");
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var AllanTest = await _context.AllanTest.FindAsync(id);
            if (AllanTest != null)
            {
                _context.AllanTest.Remove(AllanTest);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"No se encontr� ning�n elemento con el id {id}");
            }
        }

    }
}
