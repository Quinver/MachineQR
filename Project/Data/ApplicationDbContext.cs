using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Data
{
    // This class represents the database context for the application.
    // It inherits from DbContext, which is provided by Entity Framework Core.
    public class ApplicationDbContext : DbContext
    {
        // The constructor takes DbContextOptions and passes them to the base DbContext class.
        // This allows configuration of the context, such as the database provider and connection string.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // This property represents the TestEntities table in the database.
        // DbSet is a collection of entities that can be queried from the database.
        
        // Basicly these are the models that you want to store and get from a table in the database (CRUD operations)
        public DbSet<TestEntity> TestEntities { get; set; }
    }
}
