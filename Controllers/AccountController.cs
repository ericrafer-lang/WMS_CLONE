using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practice_for_wms.Data;
using practice_for_wms.Models.Entities;
using practice_for_wms.Models.ViewModels.Account;

namespace practice_for_wms.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Show login page
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        // Handle login form POST
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _context.Users
                .Include(u => u.Branch)
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null || string.IsNullOrEmpty(user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(model);
            }

            if (user.Status == UserStatus.PendingApproval)
            {
                ModelState.AddModelError(string.Empty, "Please verify your email before signing in. Check your inbox for the verification link.");
                return View(model);
            }

            if (user.Status != UserStatus.Active)
            {
                ModelState.AddModelError(string.Empty, "This account is not active. Contact an administrator.");
                return View(model);
            }

            var hasher = new PasswordHasher<User>();
            var verification = hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

            if (verification == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}".Trim()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                });

            user.LastLogin = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Dashboard");
        }

        // Clicked from the verification email sent by UserManagementController.Create
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmail(int userId, string token)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null || string.IsNullOrEmpty(user.EmailVerificationToken))
            {
                TempData["StatusMessageType"] = "error";
                TempData["StatusMessage"] = "This verification link is invalid.";
                return RedirectToAction(nameof(Login));
            }

            if (user.EmailVerificationToken != token)
            {
                TempData["StatusMessageType"] = "error";
                TempData["StatusMessage"] = "This verification link is invalid.";
                return RedirectToAction(nameof(Login));
            }

            if (user.EmailVerificationTokenExpiresAt is null || user.EmailVerificationTokenExpiresAt < DateTime.Now)
            {
                TempData["StatusMessageType"] = "error";
                TempData["StatusMessage"] = "This verification link has expired. Ask an administrator to create your account again.";
                return RedirectToAction(nameof(Login));
            }

            user.Status = UserStatus.Active;
            user.EmailVerificationToken = null;
            user.EmailVerificationTokenExpiresAt = null;
            user.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            TempData["StatusMessageType"] = "success";
            TempData["StatusMessage"] = "Email verified! You can now sign in.";
            return RedirectToAction(nameof(Login));
        }

        // Handle logout
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
