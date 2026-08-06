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
        [ValidateAntiForgeryToken]
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

            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, create.Password);

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

            return RedirectToAction(nameof(Index));
        }

        private static string GenerateVerificationToken()
        {
            // URL-safe random token (32 bytes -> 43 base64url chars, no padding).
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
                .Replace('+', '-')
                .Replace('/', '_')
                .TrimEnd('=');
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
