using Microsoft.EntityFrameworkCore;
using practice_for_wms.Models.Entities;

namespace practice_for_wms.Data
{
    public class ApplicationDbContextcs : DbContext
    {
        public ApplicationDbContextcs(DbContextOptions<ApplicationDbContextcs> options) : base(options)
        {   
        }

        public DbSet<Users> Users { get; set; }
    }
}
