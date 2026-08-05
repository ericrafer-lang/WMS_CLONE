using Microsoft.AspNetCore.Mvc;

namespace practice_for_wms.Controllers
{
    public class RestockRequestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
