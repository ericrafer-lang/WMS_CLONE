using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practice_for_wms.Data;

namespace practice_for_wms.Controllers
{
    public class ProductRequestController : Controller
    {
        private readonly ApplicationDbContext _context;

        // TODO: replace with real logged-in user once auth is added
        private const int CurrentUserId = 1;

        public ProductRequestController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var requests = await _context.Requests
                .Include(r => r.Product)
                    .ThenInclude(p => p!.Supplier)
                .Include(r => r.RequestedBy)
                .Include(r => r.ApprovedBy) 
                .Include(r => r.Supplier)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            var productSupplierMap = new Dictionary<int, List<object>>();

            foreach (var req in requests)
            {
                if (req.Product == null) continue;
                if (productSupplierMap.ContainsKey(req.ProductId)) continue;

                var suppliers = await _context.ProductSuppliers
                    .Where(ps => ps.ProductId == req.ProductId)
                    .Include(ps => ps.Supplier)
                    .Select(ps => ps.Supplier)
                    .ToListAsync();

                var allSuppliers = new List<object>();

                if (req.Product.Supplier != null)
                {
                    allSuppliers.Add(new { id = req.Product.Supplier.Id, name = req.Product.Supplier.SupplierName });
                }

                foreach (var s in suppliers)
                {
                    if (s != null && !allSuppliers.Any(x => ((dynamic)x).id == s.Id))
                    {
                        allSuppliers.Add(new { id = s.Id, name = s.SupplierName });
                    }
                }

                productSupplierMap[req.ProductId] = allSuppliers;
            }
            ViewBag.ProductSuppliers = productSupplierMap;
            return View(requests);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id, int? quantityApproved, int? supplierId, string? reviewNotes)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                TempData["Error"] = "Request not found.";
                return RedirectToAction(nameof(Index));
            }

            var reviewer = await _context.Users.FindAsync(CurrentUserId);
            if (reviewer == null)
            {
                TempData["Error"] = "Current user not found.";
                return RedirectToAction(nameof(Index));
            }

            request.Status = "Approved";
            request.QuantityApproved = quantityApproved ?? request.Quantity;
            request.SupplierId = supplierId;
            request.ReviewNotes = reviewNotes;
            request.ApprovedById = reviewer.Id;
            request.ReviewedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id, string? reviewNotes)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                TempData["Error"] = "Request not found.";
                return RedirectToAction(nameof(Index));
            }

            var reviewer = await _context.Users.FindAsync(CurrentUserId);
            if (reviewer == null)
            {
                TempData["Error"] = "Current user not found.";
                return RedirectToAction(nameof(Index));
            }

            request.Status = "Rejected";
            request.ReviewNotes = reviewNotes;
            request.ApprovedById = reviewer.Id;
            request.ReviewedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}