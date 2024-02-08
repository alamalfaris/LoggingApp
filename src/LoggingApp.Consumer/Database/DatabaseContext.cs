using LoggingApp.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoggingApp.Consumer.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();

                string connectionString = $"Server={configuration["DbConnection:Server"]};Database={configuration["DbConnection:Catalog"]};MultipleActiveResultSets=True;Trusted_Connection=true;TrustServerCertificate=True;";
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        public DbSet<LogEntity> LogHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogEntity>(e =>
            {
                e.HasKey(e => e.Id);
                e.ToTable("LogHistories");
            });
        }
    }
}
