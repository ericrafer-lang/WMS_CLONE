using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practice_for_wms.Data;
using practice_for_wms.Models.Entities;
using practice_for_wms.Models.ViewModels.UserManagement;
using practice_for_wms.Services;
using System.Security.Cryptography;

namespace practice_for_wms.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class UserManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public UserManagementController(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = new UserManagementIndexViewModel
            {
                Users = await _context.Users.Include(u => u.Branch).ToListAsync(),
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

            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, createUserInput.Password);

            // Account stays PendingApproval (can't log in - see AccountController.Login)
            // until the user clicks the verification link we email them.
            user.EmailVerificationToken = GenerateVerificationToken();
            user.EmailVerificationTokenExpiresAt = DateTime.Now.AddHours(24);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var verificationLink = Url.Action(
                "VerifyEmail", "Account",
                new { userId = user.Id, token = user.EmailVerificationToken },
                protocol: Request.Scheme);

            await _emailSender.SendEmailVerificationAsync(
                user.Email,
                $"{user.FirstName} {user.LastName}".Trim(),
                verificationLink!);

            TempData["Success"] = "User invited! They'll need to verify their email before they can sign in.";
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

        private static string GenerateVerificationToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
                .Replace('+', '-')
                .Replace('/', '_')
                .TrimEnd('=');
        }
    }
}