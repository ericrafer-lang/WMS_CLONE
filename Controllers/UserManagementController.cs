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
        public async Task<IActionResult> Create([Bind(Prefix = "CreateUser")] CreateUserViewModel createUserInput)
        {
            if (!ModelState.IsValid)
            {
                var model = new UserManagementIndexViewModel
                {
                    Users = await _context.Users.Include(u => u.Branch).ToListAsync(),
                    Branches = await _context.Branches.ToListAsync(),
                    CreateUser = createUserInput
                };
                return View("Index", model);
            }

            User user = new User
            {
                FirstName = createUserInput.FirstName,
                MiddleName = createUserInput.MiddleName,
                LastName = createUserInput.LastName,
                Email = createUserInput.Email,
                Role = createUserInput.Role,
                BranchId = createUserInput.BranchId,
                Status = UserStatus.PendingApproval,
                CreatedAt = DateTime.Now
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            TempData["Success"] = "User created successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([Bind(Prefix = "UpdateUser")] UpdateUserViewModel updateUserInput)
        {
            if (!ModelState.IsValid)
            {
                var model = new UserManagementIndexViewModel
                {
                    Users = await _context.Users.Include(u => u.Branch).ToListAsync(),
                    Branches = await _context.Branches.ToListAsync(),
                    UpdateUser = updateUserInput
                };
                return View("Index", model);
            }

            var user = await _context.Users.FindAsync(updateUserInput.Id);
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = updateUserInput.FirstName;
            user.MiddleName = updateUserInput.MiddleName;
            user.LastName = updateUserInput.LastName;
            user.Email = updateUserInput.Email;
            user.Role = updateUserInput.Role;
            user.BranchId = updateUserInput.BranchId;
            user.Status = updateUserInput.Status;
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
