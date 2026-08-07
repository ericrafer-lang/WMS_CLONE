using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practice_for_wms.Data;
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
        public async Task<IActionResult> Index(string searchString, string categoryFilter)
        {
            ViewBag.SearchString = searchString;
            ViewBag.CategoryFilter = categoryFilter;

            ViewBag.Suppliers = await _context.Suppliers.Where(s => s.Status == "Active").ToListAsync();

            var productsQuery = _context.Products
                .Include(p => p.Supplier)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(searchString) || (p.Supplier != null && p.Supplier.SupplierName.Contains(searchString)));
            }

            if (!string.IsNullOrEmpty(categoryFilter) && categoryFilter != "All Categories")
            {
                productsQuery = productsQuery.Where(p => p.Category == categoryFilter);
            }

            var products = await productsQuery.ToListAsync();
            return View(products);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(string name, string category, string unit, int qty, decimal price,
             string? description, int? supplierId)
        {
            var product = new Product
            {
                Name = name,
                Category = category,
                Unit = unit ?? "pcs",
                qty = qty,
                Price = price,
                Description = description,
                SupplierId = supplierId
            };
            _context.Products.Add(product);
            _context.SaveChanges();

            TempData["Success"] = "Product added successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, string name, string category, string unit, int qty, decimal price, string? description, int? supplierId)
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
            product.SupplierId = supplierId;

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