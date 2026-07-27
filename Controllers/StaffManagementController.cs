using Microsoft.AspNetCore.Mvc;

namespace practice_for_wms.Controllers
{
    public class StaffManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
