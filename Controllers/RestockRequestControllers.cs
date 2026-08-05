using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practice_for_wms.Data;
using practice_for_wms.Models.Entities;

namespace practice_for_wms.Controllers
{
    public class RestockRequestController : Controller
    {
        private readonly ApplicationDbContext _context;

        // TODO: replace with real logged-in user once auth is added
        private const int CurrentUserId = 1;

        public RestockRequestController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var requests = await _context.Requests
                .Include(r => r.Product)
                    .ThenInclude(p => p!.Supplier)
                .Include(r => r.RequestedBy)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            ViewBag.Products = await _context.Products.ToListAsync();

            return View(requests);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int ProductId, int Quantity, string Unit, string Priority, string Reason, string? Notes)
        {
            if (Quantity <= 0)
            {
                TempData["Error"] = "Quantity must be greater than 0.";
                return RedirectToAction(nameof(Index));
            }

            var product = await _context.Products.FindAsync(ProductId);
            if (product == null)
            {
                TempData["Error"] = $"Please select a valid product. (Received ProductId = {ProductId})";
                return RedirectToAction(nameof(Index));
            }

            var currentUser = await _context.Users.FindAsync(CurrentUserId);
            if (currentUser == null)
            {
                TempData["Error"] = "Current user not found.";
                return RedirectToAction(nameof(Index));
            }

            var request = new Request
            {
                ProductId = product.Id,
                Quantity = Quantity,
                Unit = Unit,
                Priority = Priority,
                Reason = Reason,
                Notes = Notes,
                RequestedById = currentUser.Id,
                BranchId = currentUser.BranchId,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}