using CRUDGenerator.Models;

namespace CRUDGenerator.Interfaces
{
    public interface IGeneratorService
    {
        public Task<List<DBColums>> GetAllColums(string table);
        public bool CreateCRUD(string table);
    }
}
