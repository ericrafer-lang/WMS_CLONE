using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace practice_for_wms.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class StaffManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
