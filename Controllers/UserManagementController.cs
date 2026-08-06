using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practice_for_wms.Data;
using practice_for_wms.Models.Entities;
using practice_for_wms.Models.ViewModels.UserManagement;

namespace practice_for_wms.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var viewModel = new UserManagementIndexViewModel
            {
                Users = await _context.Users
                    .Include(u => u.Branch)
                    .ToListAsync(),

                Branches = await _context.Branches.ToListAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserManagementIndexViewModel CreateUser)
        {
            
            if (!ModelState.IsValid)
            {
                CreateUser.Users = await _context.Users
                        .Include(u => u.Branch)
                        .ToListAsync();

                CreateUser.Branches = await _context.Branches.ToListAsync();
                return View("Index", CreateUser);
            }

            var create = CreateUser.CreateUser;

            User user = new User
            {
                FirstName = create.FirstName,
                MiddleName = create.MiddleName,
                LastName = create.LastName,
                Email = create.Email,
                Role = create.Role,
                BranchId = create.BranchId,

                Status = UserStatus.PendingApproval, // UserStatus from Models/Entities/User.cs (for referce kasi nakakalito)
                CreatedAt = DateTime.Now
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Sent successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UserManagementIndexViewModel UpdateUser)
        {
            if (!ModelState.IsValid)
            {
                UpdateUser.Users = await _context.Users
                        .Include(u => u.Branch)
                        .ToListAsync();
                UpdateUser.Branches = await _context.Branches.ToListAsync();
                return View("Index", UpdateUser);
            }

            var update = UpdateUser.UpdateUser;

            var user = await _context.Users.FindAsync(update.Id);
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = update.FirstName;
            user.MiddleName = update.MiddleName;
            user.LastName = update.LastName;
            user.Email = update.Email;
            user.Role = update.Role;
            user.BranchId = update.BranchId;
            user.Status = update.Status;
            user.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            TempData["Success"] = "User updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
