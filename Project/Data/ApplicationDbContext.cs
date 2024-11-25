using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Data
{
    // This class represents the database context for the application.
    // It inherits from DbContext, which is provided by Entity Framework Core.
    public class ApplicationDbContext : IdentityDbContext
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
        public DbSet<MachineModel> MachineModels { get; set; }
        public DbSet<MachinePdf> MachinePdfs { get; set; }

        // Configure the relationships between the entities.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MachinePdf>()
                .HasOne(p => p.MachineModel)
                .WithMany(m => m.MachinePdfs)
                .HasForeignKey(p => p.MachineModelId)
                .OnDelete(DeleteBehavior.Cascade); // Optional: Configure delete behavior
        }
    }
}
