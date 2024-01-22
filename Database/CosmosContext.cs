using CosmosDemo.Configuration;
using CosmosDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace CosmosDemo.Database
{
    public class CosmosContext : DbContext
    {
        private readonly FunctionConfiguration _config;
        public CosmosContext(FunctionConfiguration config)
        {
            _config = config;
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<AuthorNew> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Book>().ToContainer("Book")
                .HasNoDiscriminator().HasPartitionKey("Id");

            modelBuilder
               .Entity<AuthorNew>().ToContainer("AuthorNew")
               .HasNoDiscriminator().HasPartitionKey("Id");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseCosmos(
                accountEndpoint: _config.CosmosAccountEndpoint,
                accountKey: _config.CosmosAccountKey,
                databaseName: _config.CosmosDatabaseName);
        }
    }
}
