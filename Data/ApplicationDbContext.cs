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
    }
}
