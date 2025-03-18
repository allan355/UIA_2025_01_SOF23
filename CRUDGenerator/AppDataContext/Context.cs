using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using CRUDGenerator.Models;

namespace CRUDGenerator.AppDataContext
{
    public class Context : DbContext
    {
        // DbSettings field to store the connection string
        private readonly DbSettings _dbsettings;

        // Constructor to inject the DbSettings model
        public Context(IOptions<DbSettings> dbSettings)
        {
            _dbsettings = dbSettings.Value;
        }


        // DbSet property to represent the Sample table
        public DbSet<DBColums> DBColums { get; set; }
        //Add here property:

        // Configuring the database provider and connection string

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_dbsettings.ConnectionString);
        }

        // Configuring the model for the Sample entity
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<DBColums>().ToTable("DBColums").HasKey(x => x.ORDINAL_POSITION);

            //Add here modelbuilder:

        }
    }
}