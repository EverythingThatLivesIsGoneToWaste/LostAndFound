using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFound.Controllers
{
    [Authorize(Roles = "User, Admin")]
    public class ItemsController : Controller
    {
        // GET: Items/GetItems
        public ActionResult GetItems()
        {
            // TODO
            return View();
        }

        // POST: Items/CreateItem (form)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateItem(/*ItemModel model DTO*/)
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

        // POST: Items/DeleteItem/[int]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(int id)
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
