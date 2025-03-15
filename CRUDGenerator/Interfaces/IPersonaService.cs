using CRUDGenerator.Contracts;
using CRUDGenerator.Models;

namespace CRUDGenerator.Interfaces
{
    public interface IPersonaServices
    {
        IEnumerable<ModelBase> GetAll();
        Task<ModelBase> GetByIdAsync(Guid id);
        Task CreateAsync(ContractBase request);
        Task UpdateAsync(Guid id, ContractBase request);
        Task DeleteAsync(Guid id);
    }
}