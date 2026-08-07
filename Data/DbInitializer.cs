using Microsoft.AspNetCore.Identity;
using practice_for_wms.Models.Entities;

namespace practice_for_wms.Data
{
    public static class DbInitializer
    {
        // Seeds a default branch + Admin account so the app is loggable-into
        // right after the first migration. Change/remove this account before
        // using real data.
        public const string DefaultAdminEmail = "admin@gmail.com";
        public const string DefaultAdminPassword = "Admin@123";

        public static void Seed(ApplicationDbContext context)
        {
            // Assumes migrations have already been applied (dotnet ef database update).
            if (context.Users.Any())
            {
                return;
            }

            var branch = new Branch
            {
                BranchName = "Main Branch",
                BranchAddress = "N/A",
                PhoneNumber = "N/A",
                Status = BranchStatus.Active,
                CreatedAt = DateTime.Now,
            };
            context.Branches.Add(branch);
            context.SaveChanges();

            var admin = new User
            {
                FirstName = "System",
                MiddleName = "",
                LastName = "Admin",
                Email = DefaultAdminEmail,
                Role = "Admin",
                BranchId = branch.Id,
                Status = UserStatus.Active,
                CreatedAt = DateTime.Now,
            };

            var hasher = new PasswordHasher<User>();
            admin.PasswordHash = hasher.HashPassword(admin, DefaultAdminPassword);

            context.Users.Add(admin);
            context.SaveChanges();
        }
    }
}
