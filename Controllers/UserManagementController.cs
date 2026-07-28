using Microsoft.AspNetCore.Mvc;
using practice_for_wms.Models.Entities;

namespace practice_for_wms.Controllers
{
    public class UserManagementController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Create(Users users)
        //{
        //    ff
        //}
    }
}
