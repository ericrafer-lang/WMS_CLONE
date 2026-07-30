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
        public IActionResult Index()
        {
            ViewBag.Suppliers = _context.Suppliers.Where(s => s.Status == "Active").ToList();
            var products = _context.Products
                .Include(p => p.Supplier)
                .ToList();
            return View(products);
        }

        public IActionResult Add()
        {
            return View();
        }
    }
}
