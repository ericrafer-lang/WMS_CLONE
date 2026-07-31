using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practice_for_wms.Data;
using practice_for_wms.Models.Entities;
using practice_for_wms.ViewModels.Branches;

namespace practice_for_wms.Controllers
{
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
        public async Task<IActionResult> Create(BranchIndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Branches = await _context.Branches.ToListAsync();
                return View("Index", model);
            }

            var create = model.CreateBranch;

            Branch branch = new Branch
            {
                BranchName = create.BranchName,
                BranchAddress = create.BranchAddress,
                PhoneNumber = create.PhoneNumber,

                Status = BranchStatus.Active,
                CreatedAt = DateTime.UtcNow
            };
            _context.Branches.Add(branch);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
