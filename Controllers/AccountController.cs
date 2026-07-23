using Microsoft.AspNetCore.Mvc;

namespace practice_for_wms.Controllers
{
    public class AccountController : Controller
    {
        // Show login page
        public IActionResult Login()
        {
            return View(); // Views/Account/Login.cshtml
        }

        // Handle login form POST
        [HttpPost]
        public IActionResult DoLogin(string username, string password)
        {
            // No database/auth yet — just redirect
            return RedirectToAction("Index", "Dashboard");
        }
    }
}
