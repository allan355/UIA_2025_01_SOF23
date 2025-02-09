using AutoMapper;
using CRUDGenerator.AppDataContext;
using CRUDGenerator.Interfaces;
using CRUDGenerator.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDGenerator.Services
{
    public class GeneratorService : IGeneratorService
    {
        private readonly Context _context;
        private readonly ILogger<DBColums> _logger;
        private readonly IMapper _mapper;

        public GeneratorService(Context context, ILogger<DBColums> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<DBColums>> GetAllColums(string table)
        {
            var sample = await _context.DBColums.Where(x => x.TABLE_NAME == table).ToListAsync();

            foreach(var s in sample)
            {
                Console.WriteLine(s.TABLE_NAME);
            }

            if (sample == null)
            {
                throw new KeyNotFoundException($"No se encontró ningún elemento Sapmple con Id {table}.");
            }
            return sample;
        }
    }
}
