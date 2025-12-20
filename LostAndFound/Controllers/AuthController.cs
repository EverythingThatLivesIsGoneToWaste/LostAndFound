using LostAndFound.DTOs;
using LostAndFound.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFound.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _authService.AuthenticateAsync(model.Login, model.Password);

            if (user != null)
            {
                await _authService.SignInAsync(user);
                return RedirectToAction("Index", "Dashboard");
            }

            ModelState.AddModelError(string.Empty, "Неверный логин или пароль");
            return View(model);
        }
    }
}
