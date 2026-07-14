using Microsoft.AspNetCore.Mvc;

namespace practice_for_wms.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
