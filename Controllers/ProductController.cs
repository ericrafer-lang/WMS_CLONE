using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practice_for_wms.Data;
using practice_for_wms.Models;
namespace practice_for_wms.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Suppliers = await _context.Suppliers.Where(s => s.Status == "Active").ToListAsync();
            var products = await _context.Products
                .Include(p => p.Supplier)
                .ToListAsync();
            return View(products);
        }

        public IActionResult Add()
        {
            return View();
        }
    }
}
