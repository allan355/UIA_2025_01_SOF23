using AutoMapper;
using CRUDGenerator.AppDataContext;
using CRUDGenerator.Interfaces;
using CRUDGenerator.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Runtime.InteropServices.Marshalling.IIUnknownCacheStrategy;

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

            foreach (var s in sample)
            {
                Console.WriteLine(s.TABLE_NAME);
            }

            if (sample == null)
            {
                throw new KeyNotFoundException($"No se encontró ningún elemento Sapmple con Id {table}.");
            }
            return sample;
        }

        public bool CreateCRUD(string table)
        {
            var tableInfo = _context.DBColums.Where(x => x.TABLE_NAME == table).ToList();
            var NombreTable = tableInfo.FirstOrDefault()?.TABLE_NAME;
            bool flag = true;
            if (!CreateModel(NombreTable, tableInfo))
                flag = false;
            if (!CreateController(NombreTable, tableInfo))
                flag = false;
            if (!CreateContract(NombreTable, tableInfo))
                flag = false;
            if (!CreateIService(NombreTable, tableInfo))
                flag = false;
            if (!CreateService(NombreTable, tableInfo))
                flag = false;
            if (!AddContextValues(NombreTable, tableInfo))
                flag = false;
            if (!AddProgramValues(NombreTable, tableInfo))
                flag = false;
            if (!AddMAppingValues(NombreTable, tableInfo))
                flag = false;

            return true;
        }


        public bool CreateModel(string NombreTable, List<DBColums> tableInfo)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "_templates", "ModelTemplate.txt");
            if (!System.IO.File.Exists(filePath))
            {
                throw new Exception("El archivo no existe");
            }

            var lines = System.IO.File.ReadAllText(filePath);
            // Replace placeholders with actual values
            lines = lines.Replace("{{Table}}", NombreTable);
            var properties = "";
            foreach (var item in tableInfo)
            {
                properties += $"public {ConvertSQLTypeToCType(item.DATA_TYPE)} {item.COLUMN_NAME} {{ get; set; }}\n";
            }
            lines = lines.Replace("{{Properties}}", properties);

            // Save the generated content to a new file in the Models folder
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Models", $"{NombreTable}.cs");
            System.IO.File.WriteAllText(outputFilePath, lines);

            return true;
        }
        public bool CreateController(string NombreTable, List<DBColums> tableInfo)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "_templates", "ControllerTemplate.txt");
            if (!System.IO.File.Exists(filePath))
            {
                throw new Exception("El archivo no existe");
            }

            var lines = System.IO.File.ReadAllText(filePath);
            // Replace placeholders with actual values
            lines = lines.Replace("{{Table}}", NombreTable);

            // Save the generated content to a new file in the Models folder
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Controllers", $"{NombreTable}Controller.cs");
            System.IO.File.WriteAllText(outputFilePath, lines);

            return true;
        }

        public bool CreateContract(string NombreTable, List<DBColums> tableInfo)
        {

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "_templates", "ContractTemplate.txt");
            if (!System.IO.File.Exists(filePath))
            {
                throw new Exception("El archivo no existe");
            }

            var lines = System.IO.File.ReadAllText(filePath);
            // Replace placeholders with actual values
            lines = lines.Replace("{{Table}}", NombreTable);
            var properties = "";
            foreach (var item in tableInfo)
            {
                properties += $"public {ConvertSQLTypeToCType(item.DATA_TYPE)} {item.COLUMN_NAME} {{ get; set; }}\n";
            }
            lines = lines.Replace("{{Properties}}", properties);

            // Save the generated content to a new file in the Models folder
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Contracts", $"{NombreTable}Request.cs");
            System.IO.File.WriteAllText(outputFilePath, lines);

            return true;
        }

        public bool CreateService(string NombreTable, List<DBColums> tableInfo)
        {

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "_templates", "ServicesTemplate.txt");
            if (!System.IO.File.Exists(filePath))
            {
                throw new Exception("El archivo no existe");
            }

            var lines = System.IO.File.ReadAllText(filePath);
            // Replace placeholders with actual values
            lines = lines.Replace("{{Table}}", NombreTable);
            var ifproperties = "";
            foreach (var item in tableInfo)
            {
                ifproperties += $"if (request.{item.COLUMN_NAME} != null)\n";
                ifproperties += "{\n";
                ifproperties += $"    {NombreTable}.{item.COLUMN_NAME} = request.{item.COLUMN_NAME};\n";
                ifproperties += "}\n";
            }
            lines = lines.Replace("{{IfProperties}}", ifproperties);
            // Save the generated content to a new file in the Models folder
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Services", $"{NombreTable}Service.cs");
            System.IO.File.WriteAllText(outputFilePath, lines);

            return true;
        }
        public bool CreateIService(string NombreTable, List<DBColums> tableInfo)
        {

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "_templates", "IServicesTemplate.txt");
            if (!System.IO.File.Exists(filePath))
            {
                throw new Exception("El archivo no existe");
            }

            var lines = System.IO.File.ReadAllText(filePath);
            // Replace placeholders with actual values
            lines = lines.Replace("{{Table}}", NombreTable);
          
            // Save the generated content to a new file in the Models folder
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Interfaces", $"I{NombreTable}Service.cs");
            System.IO.File.WriteAllText(outputFilePath, lines);

            return true;
        }

        public bool AddContextValues(string NombreTable, List<DBColums> tableInfo)
        {

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "AppDataContext", "Context.cs");
            if (!System.IO.File.Exists(filePath))
            {
                throw new Exception("El archivo no existe");
            }

            var lines = System.IO.File.ReadAllLines(filePath);
            var newlines = new List<string>();
            // Replace placeholders with actual values
            var property = $"public DbSet<{NombreTable}> {NombreTable} {{ get; set; }}";
            if (lines.Contains(property))
            {
                return true;
            }
            foreach (var item in lines)
            {
                if (item.Trim().Equals("//Add here property:"))
                {
                    newlines.Add(item);
                    newlines.Add(property);
                }
                else
                {
                    newlines.Add(item);
                }
            }
            lines = newlines.ToArray();
            newlines = new List<string>();
            var modelBuilder = $"modelBuilder.Entity<{NombreTable}>().ToTable(\"{NombreTable}\").HasKey(x => x.{tableInfo.Where(x => x.ORDINAL_POSITION == 1).FirstOrDefault()?.COLUMN_NAME});";
            foreach (var item in lines)
            {
                if (item.Trim().Equals("//Add here modelbuilder:"))
                {
                    newlines.Add(item);
                    newlines.Add(modelBuilder);
                }
                else
                {
                    newlines.Add(item);
                }
            }

            // Save the generated content to a new file in the Models folder
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "AppDataContext", "Context.cs");
            System.IO.File.WriteAllText(outputFilePath, string.Join('\n', newlines));

            return true;
        }
        public bool AddProgramValues(string NombreTable, List<DBColums> tableInfo)
        {

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "", "Program.cs");
            if (!System.IO.File.Exists(filePath))
            {
                throw new Exception("El archivo no existe");
            }

            var lines = System.IO.File.ReadAllLines(filePath);
            var newlines = new List<string>();
            // Replace placeholders with actual values
            var property = $"builder.Services.AddScoped<I{NombreTable}Services, {NombreTable}Services>();";
            if (lines.Contains(property))
            {
                return true;
            }
            foreach (var item in lines)
            {
                if (item.Trim().Equals("//Add here services:"))
                {
                    newlines.Add(item);
                    newlines.Add(property);
                }
                else
                {
                    newlines.Add(item);
                }
            }

            // Save the generated content to a new file in the Models folder
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "", "Program.cs");
            System.IO.File.WriteAllText(outputFilePath, string.Join('\n', newlines));

            return true;
        }
        public bool AddMAppingValues(string NombreTable, List<DBColums> tableInfo)
        {

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Mapping", "AutoMapperProfile.cs");
            if (!System.IO.File.Exists(filePath))
            {
                throw new Exception("El archivo no existe");
            }

            var lines = System.IO.File.ReadAllLines(filePath);
            var newlines = new List<string>();
            // Replace placeholders with actual values
            var property = $"CreateMap<{NombreTable}Request, {NombreTable}>();";
            if (lines.Contains(property))
            {
                return true;
            }
            foreach (var item in lines)
            {
                if (item.Trim().Equals("//add more mappings here:"))
                {
                    newlines.Add(item);
                    newlines.Add(property);
                }
                else
                {
                    newlines.Add(item);
                }
            }

            // Save the generated content to a new file in the Models folder
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Mapping", "AutoMapperProfile.cs");
            System.IO.File.WriteAllText(outputFilePath, string.Join('\n', newlines));

            return true;
        }

        public string ConvertSQLTypeToCType(string SQLType)
        {
            return SQLType.ToLower() switch
            {
                "int" => "int",
                "varchar" => "string",
                "datetime" => "DateTime",
                "bit" => "bool",
                "decimal" => "decimal",
                "uniqueidentifier" => "Guid",
                _ => "string",
            };
        }
    }
}
