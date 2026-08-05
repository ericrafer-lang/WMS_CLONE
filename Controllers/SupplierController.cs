using Microsoft.AspNetCore.Mvc;
using practice_for_wms.Data;

using practice_for_wms.Models.Entities;


namespace practice_for_wms.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupplierController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var suppliers = _context.Suppliers.ToList();
            return View(suppliers);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(string supplierName, string contactPerson, string supplierEmail,
            string phone, string address)
        {
            var supplier = new Supplier
            {
                SupplierName = supplierName,
                ContactPerson = contactPerson ?? "",
                SupplierEmail = supplierEmail ?? "",
                Phone = phone ?? "",
                Address = address ?? "",
                Status = "Active"
            };
            _context.Suppliers.Add(supplier);
            _context.SaveChanges();
            TempData["Success"] = "Supplier added successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, string name, string contactPerson, string supplierEmail, string phone, string address, string status)
        {
            var supplier = _context.Suppliers.Find(id);
            if (supplier == null)
            {
                return NotFound();
            }

            supplier.SupplierName = name;
            supplier.ContactPerson = contactPerson;
            supplier.SupplierEmail = supplierEmail;
            supplier.Phone = phone ?? "";
            supplier.Address = address ?? "";
            supplier.Status = status ?? "Inactive";

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var supplier = _context.Suppliers.Find(id);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
