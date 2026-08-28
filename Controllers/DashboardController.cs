using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practice_for_wms.Models.ViewModels.Dashboard;
using practice_for_wms.Data;
namespace practice_for_wms.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        // A product is "Low Stock" once qty drops to or below this, and
        // "Out of Stock" at exactly 0. Tune this if the team wants a
        // different threshold.
        private const int LowStockThreshold = 10;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Select(p => new { p.Category, p.qty })
                .ToListAsync();

            var categoryTotals = products
                .GroupBy(p => string.IsNullOrWhiteSpace(p.Category) ? "Uncategorized" : p.Category)
                .Select(g => new { Category = g.Key, Total = g.Sum(p => p.qty) })
                .OrderByDescending(g => g.Total)
                .ToList();

            var vm = new DashboardViewModel
            {
                TotalProducts = products.Count,
                TotalSuppliers = _context.Suppliers.Count(),
                TotalUsers = _context.Users.Count(),
                TotalBranches = _context.Branches.Count(),

                AvailableStock = products.Sum(p => p.qty),
                OutOfStockCount = products.Count(p => p.qty == 0),
                LowStockCount = products.Count(p => p.qty > 0 && p.qty <= LowStockThreshold),
                GoodStockCount = products.Count(p => p.qty > LowStockThreshold),

                CategoryLabels = categoryTotals.Select(c => c.Category).ToList(),
                CategoryQuantities = categoryTotals.Select(c => c.Total).ToList(),
            };

            return View(vm);
        }
    }
}
