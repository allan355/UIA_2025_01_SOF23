using AutoMapper;
using CRUDGenerator.Contracts;
using CRUDGenerator.Interfaces;
using CRUDGenerator.Models;
using CRUDGenerator.AppDataContext;

namespace CRUDGenerator.Services
{
    public class SampleServices : IServices
    {
        private readonly Context _context;
        private readonly ILogger<SampleServices> _logger;
        private readonly IMapper _mapper;
        public SampleServices(Context context, ILogger<SampleServices> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task CreateAsync(CreateSampleRequest request)
        {
            try
            {
                var sample = _mapper.Map<Sample>(request);
                sample.CreatedAt = DateTime.UtcNow;
                _context.Samples.Add(sample);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al crear el elemento Sample.");
                throw new Exception("Ocurrió un error al crear el elemento Sample.");
            }
        }       

        public IEnumerable<Sample> GetAll()
        {
            var sample = _context.Samples.ToList();
            if (sample == null)
            {
                throw new Exception("No se encontraron elementos Sample");
            }
            return sample;
        }

        public async Task<Sample> GetByIdAsync(Guid id)
        {
            var sample = await _context.Samples.FindAsync(id);
            if (sample == null)
            {
                throw new KeyNotFoundException($"No se encontró ningún elemento Sapmple con Id {id}.");
            }
            return sample;
        }

        public async Task UpdateAsync(Guid id, UpdateSampleRequest request)
        {
            try
            {
                var sample = await _context.Samples.FindAsync(id);
                if (sample == null)
                {
                    throw new Exception($"No se encontró el elemento Sample con id {id}.");
                }

                if (request.Title != null)
                {
                    sample.Title = request.Title;
                }

                if (request.Description != null)
                {
                    sample.Description = request.Description;
                }

                if (request.IsComplete != null)
                {
                    sample.IsComplete = request.IsComplete.Value;
                }

                if (request.DueDate != null)
                {
                    sample.DueDate = request.DueDate.Value;
                }

                if (request.Priority != null)
                {
                    sample.Priority = request.Priority.Value;
                }

                sample.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ocurrió un error al actualizar el elemento Sample con id {id}.");
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var sample = await _context.Samples.FindAsync(id);
            if (sample != null)
            {
                _context.Samples.Remove(sample);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"No se encontró ningún elemento con el id {id}");
            }
        }
    }
}
