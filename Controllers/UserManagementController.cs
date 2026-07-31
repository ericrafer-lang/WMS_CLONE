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
            //List<User>;

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
        public async Task<IActionResult> Create(UserManagementIndexViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model.Users = await _context.Users
                        .Include(u => u.Branch)
                        .ToListAsync();

                model.Branches = await _context.Branches.ToListAsync();
                return View("Index", model);
            }

            var create = model.CreateUser;

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

            return RedirectToAction(nameof(Index));
        }
    }
}
