using Microsoft.AspNetCore.Mvc;

namespace LostAndFound.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
