using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace practice_for_wms.Controllers
{
    [Authorize(Policy = "AdminOrSupervisor")]
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
