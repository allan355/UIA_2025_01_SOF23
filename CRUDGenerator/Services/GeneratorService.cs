using AutoMapper;
using CRUDGenerator.AppDataContext;
using CRUDGenerator.Interfaces;
using CRUDGenerator.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.Intrinsics.X86;

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

        public async Task<List<string>> GetAllTables()
        {
            var sample = await _context.DBColums.Select(x => x.TABLE_NAME).Distinct().ToListAsync();

            if (sample == null)
            {
                throw new KeyNotFoundException($"No se encontró ningún elemento.");
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
            if (!CreateHTML(NombreTable, tableInfo))
                flag = false;
            if (!CreateJS(NombreTable, tableInfo))
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
                properties += $"public {ConvertSQLTypeToCType(item.DATA_TYPE)} {item.COLUMN_NAME} {{ get; set; }}{Environment.NewLine}";
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
            lines = lines.Replace("{{PK}}", tableInfo.Where(x => x.IS_PRIMARY_KEY).FirstOrDefault()?.COLUMN_NAME);
            lines = lines.Replace("{{PKType}}", ConvertSQLTypeToCType(tableInfo.Where(x => x.IS_PRIMARY_KEY).FirstOrDefault()?.DATA_TYPE));

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
            foreach (var item in tableInfo.Where(x => !x.IS_PRIMARY_KEY))
            {
                properties += $"public {ConvertSQLTypeToCType(item.DATA_TYPE)} {item.COLUMN_NAME} {{ get; set; }}{Environment.NewLine}";
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
            foreach (var item in tableInfo.Where(x => !x.IS_PRIMARY_KEY))
            {
                ifproperties += $"if (request.{item.COLUMN_NAME} != null){Environment.NewLine}";
                ifproperties += "{ "+Environment.NewLine;
                ifproperties += $"    {NombreTable}.{item.COLUMN_NAME} = request.{item.COLUMN_NAME};{Environment.NewLine}";
                ifproperties += "} " + Environment.NewLine;
            }
            lines = lines.Replace("{{IfProperties}}", ifproperties);
            lines = lines.Replace("{{PKType}}", ConvertSQLTypeToCType(tableInfo.Where(x => x.IS_PRIMARY_KEY).FirstOrDefault()?.DATA_TYPE));
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

            lines = lines.Replace("{{PKType}}", ConvertSQLTypeToCType(tableInfo.Where(x => x.IS_PRIMARY_KEY).FirstOrDefault()?.DATA_TYPE));
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
            System.IO.File.WriteAllText(outputFilePath, string.Join(Environment.NewLine, newlines));

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
            System.IO.File.WriteAllText(outputFilePath, string.Join(Environment.NewLine, newlines));

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
            var pk = tableInfo.Where(x => x.IS_PRIMARY_KEY).FirstOrDefault()?.COLUMN_NAME;
            var property = "";
            if (pk != null)
            {
                property = $"CreateMap<{NombreTable}Request, {NombreTable}>().ForMember(x=>x.{pk},o=>o.Ignore());";
            }
            else
            {
                property = $"CreateMap<{NombreTable}Request, {NombreTable}>();";
            }
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
            System.IO.File.WriteAllText(outputFilePath, string.Join(Environment.NewLine, newlines));

            return true;
        }
        public bool CreateHTML(string NombreTable, List<DBColums> tableInfo)
        {

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"_templates\FrontEnd", "HTMLTemplate.txt");
            if (!System.IO.File.Exists(filePath))
            {
                throw new Exception("El archivo no existe");
            }
            var lines = System.IO.File.ReadAllText(filePath);
            // Replace placeholders with actual values
            lines = lines.Replace("{{Table}}", NombreTable);
            lines = lines.Replace("{{ProjectName}}", "Proyeco Paradigmas 2025");
            var Properties = "";
            foreach (var item in tableInfo)
            {
                Properties += $"<th scope=\"col\">{item.COLUMN_NAME}</th>{Environment.NewLine}";
            }
            lines = lines.Replace("{{Properties}}", Properties);

            // Save the generated content to a new file in the Models folder
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), @"FrontEnd\views", $"{NombreTable}.html");
            System.IO.File.WriteAllText(outputFilePath, lines);

            return true;
        }
        public bool CreateJS(string NombreTable, List<DBColums> tableInfo)
        {

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"_templates\FrontEnd", "JSTemplate.txt");
            if (!System.IO.File.Exists(filePath))
            {
                throw new Exception("El archivo no existe");
            }

            var lines = System.IO.File.ReadAllText(filePath);
            // Replace placeholders with actual values
            lines = lines.Replace("{{Table}}", NombreTable);
            lines = lines.Replace("{{PK}}", tableInfo.Where(x => x.IS_PRIMARY_KEY).FirstOrDefault()?.COLUMN_NAME);
            lines = lines.Replace("{{URL}}", "https://localhost:7038/api");

            var loadTableProperties = "";
            var showCreateProperties = "";
            var columnsMethodProperties = "";
            var JSONProperties = "";
            foreach (var item in tableInfo)
            {
                if (item.IS_PRIMARY_KEY == true)
                {
                    loadTableProperties += $"trHTML += '<td>' + object['{item.COLUMN_NAME}'] + '</td>';{Environment.NewLine}";
                    showCreateProperties += $"'<input id=\"{item.COLUMN_NAME}\" type=\"hidden\" value=\"'+{item.COLUMN_NAME}+'\" >'+{Environment.NewLine}";
                    columnsMethodProperties += $"const {item.COLUMN_NAME} = document.getElementById(\"{item.COLUMN_NAME}\").value;{Environment.NewLine}";
                }
                else
                {
                    loadTableProperties += $"trHTML += '<td>' + object['{item.COLUMN_NAME}'] + '</td>';{Environment.NewLine}";
                    showCreateProperties += $"'<input id=\"{item.COLUMN_NAME}\" class=\"swal2-input\" placeholder=\"{item.COLUMN_NAME}\">' +{Environment.NewLine}";
                    columnsMethodProperties += $"const {item.COLUMN_NAME} = document.getElementById(\"{item.COLUMN_NAME}\").value;{Environment.NewLine}";
                    JSONProperties += $" {item.COLUMN_NAME}: {item.COLUMN_NAME},{Environment.NewLine}";
                }
            }
            int lastIndex = showCreateProperties.LastIndexOf('+');
            showCreateProperties = showCreateProperties.Substring(0, lastIndex) + ',' + showCreateProperties.Substring(lastIndex + 1);

            lines = lines.Replace("{{loadTableProperties}}", loadTableProperties);
            lines = lines.Replace("{{showCreateProperties}}", showCreateProperties);
            lines = lines.Replace("{{columnsMethodProperties}}", columnsMethodProperties);
            lines = lines.Replace("{{JSONProperties}}", JSONProperties);
            // Save the generated content to a new file in the Models folder
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), @"FrontEnd\js", $"{NombreTable}.js");
            System.IO.File.WriteAllText(outputFilePath, lines);

            return true;
        }
        public string ConvertSQLTypeToCType(string SQLType)
        {
            return SQLType.ToLower() switch
            {
                "int" => "int",
                "bigint" => "long",
                "varchar" => "string",
                "datetime" => "DateTime",
                "bit" => "bool",
                "decimal" => "decimal",
                "uniqueidentifier" => "Guid",
                "float" => "float",
                "text" => "string",
                "char" => "string",
                "nchar" => "string",
                "nvarchar" => "string",
                "ntext" => "string",
                "date" => "DateTime",
                "time" => "TimeSpan",
                "datetime2" => "DateTime",
                "datetimeoffset" => "DateTimeOffset",
                "smallint" => "short",
                "tinyint" => "byte",
                "money" => "decimal",
                "smallmoney" => "decimal",
                "real" => "float",
                "binary" => "byte[]",
                "varbinary" => "byte[]",
                "image" => "byte[]",
                "xml" => "string",
                "geography" => "string",
                "geometry" => "string",
                "hierarchyid" => "string",
                "sql_variant" => "string",
                "timestamp" => "byte[]",
                "sysname" => "string",
                _ => "string",
            };
        }
    }
}
