using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practice_for_wms.Data;
using practice_for_wms.Models;
using practice_for_wms.Models.Entities;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(string name, string category, string unit, int qty, decimal price,
             string? description)
        {
            var product = new Product
            {
                Name = name,
                Category = category,
                Unit = unit ?? "pcs",
                qty = qty,
                Price = price,
                Description = description,
            };
            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, string name, string category, string unit, int qty, decimal price, string? description)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = name;
            product.Category = category;
            product.Unit = unit ?? "pcs";
            product.qty = qty;
            product.Price = price;
            product.Description = description;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
