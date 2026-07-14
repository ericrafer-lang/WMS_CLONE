using Microsoft.AspNetCore.Mvc;

namespace practice_for_wms.Controllers
{
    public class UserManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
