using Microsoft.EntityFrameworkCore;
using practice_for_wms.Models.Entities;

namespace practice_for_wms.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<MyTask> MyTasks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // MyTask: two FK references to Users and Branch
            modelBuilder.Entity<MyTask>()
                .HasOne(t => t.AssignedTo)
                .WithMany()
                .HasForeignKey(t => t.AssignedToId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MyTask>()
                .HasOne(t => t.Branch)
                .WithMany()
                .HasForeignKey(t => t.BranchId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        }
}
