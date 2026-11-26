using BookShelf.Helpers;
using BookShelf.Models;
using BookShelf.Repositories;
using BookShelf.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace BookShelf.Controllers;

public class AccountController : Controller
{
    UserRepository userRepository = new UserRepository();

    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Cabinet");
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (userRepository.EmailExists(model.Email, 0))
        {
            ModelState.AddModelError("Email", "Пользователь с таким email уже зарегистрирован");
            return View(model);
        }

        User user = new User();
        user.Name = model.Name;
        user.Email = model.Email;
        user.PasswordHash = PasswordHelper.HashPassword(model.Password);
        user.Role = "User";
        user.Id = userRepository.Create(user);

        await AuthHelper.SignIn(HttpContext, user);

        TempData["Success"] = "Регистрация прошла успешно! Добро пожаловать, " + user.Name + "!";
        return RedirectToAction("Index", "Cabinet");
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Cabinet");
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        User user = userRepository.GetByEmail(model.Email);

        if (user == null || !PasswordHelper.CheckPassword(model.Password, user.PasswordHash))
        {
            ModelState.AddModelError("", "Неверный email или пароль");
            return View(model);
        }

        await AuthHelper.SignIn(HttpContext, user);

        TempData["Success"] = "Вы вошли как " + user.Name;
        return RedirectToAction("Index", "Cabinet");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}
