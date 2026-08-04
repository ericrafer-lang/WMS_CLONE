using Microsoft.AspNetCore.Mvc;
using practice_for_wms.Models.ViewModels.Dashboard;
using practice_for_wms.Data;
namespace practice_for_wms.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var vm = new DashboardViewModel
            {
                TotalProducts = _context.Products.Count(),
                TotalSuppliers = _context.Suppliers.Count(),
                TotalUsers = _context.Users.Count(),
                TotalBranches = _context.Branches.Count(),
            };
            return View(vm);
        }
    }
}