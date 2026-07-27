using Microsoft.AspNetCore.Mvc;

namespace practice_for_wms.Controllers
{
    public class UserManagementController : Controller
    {
        [HttpPost]
        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost]
    }
}
