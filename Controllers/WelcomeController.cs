using Microsoft.AspNetCore.Mvc;

namespace KhumaloCraftST10152316.Controllers
{
    public class WelcomeController : Controller
    {
        public IActionResult Welcome()
        {
            return View();
        }
    }
}
