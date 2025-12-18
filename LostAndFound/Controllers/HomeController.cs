using Microsoft.AspNetCore.Mvc;

namespace LostAndFound.Controllers
{
    public class HomeController() : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return View(); //TODO Authorized user
            else
                return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
