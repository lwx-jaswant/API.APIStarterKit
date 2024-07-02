using Microsoft.AspNetCore.Mvc;

namespace UserMSDev.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult GoToLoginPage()
        {
            return RedirectToAction("Login", "Account");
        }
    }
}
