using CRUDGenerator.Contracts;
using CRUDGenerator.Models;

namespace CRUDGenerator.Interfaces
{
    public interface IServices
    {
        IEnumerable<Sample> GetAll();
        Task<Sample> GetByIdAsync(Guid id);
        Task CreateAsync(CreateSampleRequest request);
        Task UpdateAsync(Guid id, UpdateSampleRequest request);
        Task DeleteAsync(Guid id);
    }
}