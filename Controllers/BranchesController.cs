using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practice_for_wms.Data;
using practice_for_wms.Models.Entities;
using practice_for_wms.Models.ViewModels.Branches;

namespace practice_for_wms.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class BranchesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BranchesController(ApplicationDbContext context) 
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = new BranchIndexViewModel
            {
                Branches = await _context.Branches.ToListAsync()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(Prefix ="CreateBranch")] CreateBranchViewModel CreateBranchInput)
        {
            if (!ModelState.IsValid)
            {
                var model = new BranchIndexViewModel
                {
                    Branches = await _context.Branches.ToListAsync()
                };
                return View("Index", model);
            }

            Branch branch = new Branch
            {
                BranchName = CreateBranchInput.BranchName,
                BranchAddress = CreateBranchInput.BranchAddress,
                PhoneNumber = CreateBranchInput.PhoneNumber,

                Status = BranchStatus.Active,
                CreatedAt = DateTime.UtcNow
            };
            _context.Branches.Add(branch);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Branch added successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([Bind(Prefix = "UpdateBranch")] UpdateBranchViewModel updateBranchInput)
        {
            if (!ModelState.IsValid)
            {
                var model = new BranchIndexViewModel
                {
                    Branches = await _context.Branches.ToListAsync(),
                    UpdateBranch = updateBranchInput
                };
                return View("Index", model);
            }

            var branch = await _context.Branches.FindAsync(updateBranchInput.Id);
            if (branch == null)
            {
                return NotFound();
            }

            branch.BranchName = updateBranchInput.BranchName;
            branch.BranchAddress = updateBranchInput.BranchAddress;
            branch.PhoneNumber = updateBranchInput.PhoneNumber;
            //branch.Status = updateBranchInput.Status;
            //branch.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            TempData["Success"] = "Branch updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var branch = await _context.Branches.FindAsync(id);

            if (branch == null)
            {
                return NotFound();
            }

            _context.Branches.Remove(branch);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
