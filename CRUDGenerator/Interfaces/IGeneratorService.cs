using CRUDGenerator.Models;

namespace CRUDGenerator.Interfaces
{
    public interface IGeneratorService
    {
        public Task<List<string>> GetAllTables();
        public bool CreateCRUD(string table);
    }
}
