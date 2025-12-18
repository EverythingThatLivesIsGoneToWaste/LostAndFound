using Microsoft.AspNetCore.Mvc;
using LostAndFound.Services;

namespace LostAndFound.Controllers
{
    public class HomeController(IDateTime dateTime) : Controller
    {
        private readonly IDateTime _dateTime = dateTime;

        public IActionResult Index()
        {
            var serverTime = _dateTime.Now;
            if (serverTime.Hour < 12)
            {
                ViewData["Message"] = "It's morning here - Good Morning!";
            }
            else if (serverTime.Hour < 17)
            {
                ViewData["Message"] = "It's afternoon here - Good Afternoon!";
            }
            else
            {
                ViewData["Message"] = "It's evening here - Good Evening!";
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
