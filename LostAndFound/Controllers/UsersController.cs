using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFound.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class UsersController : Controller
    {
        // GET: Users/GetUsers
        public ActionResult GetUsers()
        {
            // TODO
            return View();
        }

        // POST: Users/CreateUser (form)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(/*UserModel model DTO*/)
        {
            if (!ModelState.IsValid)
                return View(/*model*/);

            // TODO
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: Users/DeleteUser/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUser(int id)
        {
            // TODO
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
