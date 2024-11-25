using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        // Add the DbSet properties for the entities that you want to include in the database.
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
