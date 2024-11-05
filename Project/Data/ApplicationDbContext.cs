using Microsoft.EntityFrameworkCore;

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
        public DbSet<TestEntity> TestEntities { get; set; }
    }

    // This class represents an entity in the TestEntities table.
    // Each instance of this class corresponds to a row in the table.
    public class TestEntity
    {
        // The Id property is the primary key for the table.
        public int Id { get; set; }

        // The Name property represents a column in the table.
        public string Name { get; set; }
    }
}
