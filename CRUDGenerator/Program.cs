// program.cs
using CRUDGenerator.AppDataContext;
using CRUDGenerator.Interfaces;
using CRUDGenerator.Middleware;
using CRUDGenerator.Models;
using CRUDGenerator.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());  // Add this line


builder.Services.Configure<DbSettings>(builder.Configuration.GetSection("DbSettings")); // Add this line
builder.Services.AddSingleton<Context>(); // Add this line


builder.Services.AddExceptionHandler<GlobalExceptionHandler>(); // Add this line

builder.Services.AddProblemDetails();  // Add this line

// Adding of login 
builder.Services.AddLogging();  //  Add this line

addScopes(builder);

var app = builder.Build();

{
    using var scope = app.Services.CreateScope(); // Add this line
    var context = scope.ServiceProvider; // Add this line
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseAuthorization();

app.MapControllers();

app.Run();



void addScopes(WebApplicationBuilder builder)
{

    builder.Services.AddScoped<IGeneratorService, GeneratorService>();
    //Add here services:
builder.Services.AddScoped<IPersonaServices, PersonaServices>();
builder.Services.AddScoped<IAllanTestServices, AllanTestServices>();
    //builder.Services.AddScoped<IServices, ProductServices>();
}