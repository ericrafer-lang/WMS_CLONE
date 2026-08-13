using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practice_for_wms.Data;
using practice_for_wms.Models.Entities;
using practice_for_wms.Models.ViewModels.StaffManagement;
using practice_for_wms.Services;
using System.Security.Claims;
using System.Security.Cryptography;

namespace practice_for_wms.Controllers
{
    [Authorize(Policy = "AdminOrSupervisor")]
    public class StaffManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public StaffManagementController(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var branchId = GetBranchFilter();

            var query = _context.Users
                .Include(u => u.Branch)
                .Where(u => u.Role != "Admin")
                .AsQueryable();

            if (branchId != null)
                query = query.Where(u => u.BranchId == branchId);

            var viewModel = new StaffManagementIndexViewModel
            {
                IsAdmin = branchId == null,
                CurrentBranch = branchId != null ? await _context.Branches.FindAsync(branchId) : null,
                Branches = await _context.Branches.ToListAsync(),
                Staff = await query.ToListAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(Prefix = "CreateStaff")] CreateStaffViewModel input)
        {
            var branchId = GetBranchFilter();

            // Admin must pick a branch from the dropdown; Supervisor's branch is forced below.
            if (branchId == null && input.BranchId == null)
                ModelState.AddModelError("CreateStaff.BranchId", "Please select a branch.");

            if (!ModelState.IsValid)
                return await ReloadIndexWithErrors(branchId, createInput: input);

            var user = new User
            {
                FirstName = input.FirstName,
                MiddleName = input.MiddleName,
                LastName = input.LastName,
                Email = input.Email,
                Role = input.Role,
                PhoneNumber = input.PhoneNumber,
                Shift = input.Shift,
                Notes = input.Notes,
                BranchId = branchId ?? input.BranchId!.Value, // Supervisor forced, Admin from form
                Status = UserStatus.PendingApproval,
                CreatedAt = DateTime.Now
            };

            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, Guid.NewGuid().ToString());

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

            TempData["Success"] = "Staff invited! They'll need to verify their email before they can sign in.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([Bind(Prefix = "UpdateStaff")] UpdateStaffViewModel input)
        {
            var branchId = GetBranchFilter();

            var user = await _context.Users.FindAsync(input.Id);
            if (user == null)
                return NotFound();

            // Supervisor can only edit staff in their own branch.
            if (branchId != null && user.BranchId != branchId)
                return NotFound();

            if (!ModelState.IsValid)
                return await ReloadIndexWithErrors(branchId, updateInput: input);

            user.FirstName = input.FirstName;
            user.MiddleName = input.MiddleName;
            user.LastName = input.LastName;
            user.Email = input.Email;
            user.Role = input.Role;
            user.PhoneNumber = input.PhoneNumber;
            user.Shift = input.Shift;
            user.Notes = input.Notes;
            user.Status = input.Status;
            user.UpdatedAt = DateTime.Now;

            // Only Admin may reassign a staff member's branch.
            if (branchId == null && input.BranchId != null)
                user.BranchId = input.BranchId.Value;

            await _context.SaveChangesAsync();
            TempData["Success"] = "Staff member updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var branchId = GetBranchFilter();
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            if (branchId != null && user.BranchId != branchId)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Returns null for Admin (no filter — sees all branches),
        // or the current user's branch id for Supervisor.
        private int? GetBranchFilter()
        {
            if (User.IsInRole("admin"))   // was "Admin" — must match DB casing
                return null;

            var claim = User.FindFirstValue("BranchId");
            return int.TryParse(claim, out var id) ? id : null;
        }

        private async Task<IActionResult> ReloadIndexWithErrors(
            int? branchId,
            CreateStaffViewModel? createInput = null,
            UpdateStaffViewModel? updateInput = null)
        {
            var query = _context.Users
                .Include(u => u.Branch)
                .Where(u => u.Role != "admin")
                .AsQueryable();

            if (branchId != null)
                query = query.Where(u => u.BranchId == branchId);

            var model = new StaffManagementIndexViewModel
            {
                IsAdmin = branchId == null,
                CurrentBranch = branchId != null ? await _context.Branches.FindAsync(branchId) : null,
                Branches = await _context.Branches.ToListAsync(),
                Staff = await query.ToListAsync(),
                CreateStaff = createInput ?? new CreateStaffViewModel(),
                UpdateStaff = updateInput ?? new UpdateStaffViewModel()
            };

            return View("Index", model);
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