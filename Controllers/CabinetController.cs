using System.Security.Claims;
using BookShelf.Helpers;
using BookShelf.Models;
using BookShelf.Repositories;
using BookShelf.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShelf.Controllers;

[Authorize]
public class CabinetController : Controller
{
    ItemRepository itemRepository = new ItemRepository();
    UserRepository userRepository = new UserRepository();

    private int GetUserId()
    {
        string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        return int.Parse(id);
    }

    public IActionResult Index()
    {
        CabinetViewModel model = new CabinetViewModel();
        model.UserName = User.Identity.Name;
        model.ItemsCount = itemRepository.CountByUser(GetUserId());

        return View(model);
    }

    public IActionResult MyItems()
    {
        List<Item> items = itemRepository.GetByUser(GetUserId());
        return View(items);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Item item)
    {
        if (!ModelState.IsValid)
        {
            return View(item);
        }

        item.UserId = GetUserId();
        itemRepository.Create(item);

        TempData["Success"] = "Книга «" + item.Title + "» добавлена!";
        return RedirectToAction("MyItems");
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        Item item = itemRepository.GetByIdAndUser(id, GetUserId());

        if (item == null)
        {
            return NotFound();
        }

        return View(item);
    }

    [HttpPost]
    public IActionResult Edit(Item item)
    {
        if (!ModelState.IsValid)
        {
            return View(item);
        }

        int rows = itemRepository.Update(item, GetUserId());

        if (rows == 0)
        {
            return NotFound();
        }

        TempData["Success"] = "Изменения сохранены!";
        return RedirectToAction("MyItems");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        Item item = itemRepository.GetByIdAndUser(id, GetUserId());

        if (item == null)
        {
            return NotFound();
        }

        return View(item);
    }

    [HttpPost]
    [ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        int rows = itemRepository.Delete(id, GetUserId());

        if (rows == 0)
        {
            return NotFound();
        }

        TempData["Success"] = "Книга удалена.";
        return RedirectToAction("MyItems");
    }

    [HttpGet]
    public IActionResult Profile()
    {
        User user = userRepository.GetById(GetUserId());

        ProfileViewModel model = new ProfileViewModel();
        model.Name = user.Name;
        model.Email = user.Email;
        model.CreatedAt = user.CreatedAt;
        model.Role = user.Role;

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Profile(ProfileViewModel model)
    {
        int userId = GetUserId();
        User user = userRepository.GetById(userId);

        model.CreatedAt = user.CreatedAt;
        model.Role = user.Role;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (userRepository.EmailExists(model.Email, userId))
        {
            ModelState.AddModelError("Email", "Этот email уже занят другим пользователем");
            return View(model);
        }

        userRepository.UpdateProfile(userId, model.Name, model.Email);

        user.Name = model.Name;
        user.Email = model.Email;
        await AuthHelper.SignIn(HttpContext, user);

        TempData["Success"] = "Профиль сохранён!";
        return RedirectToAction("Profile");
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    public IActionResult ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        int userId = GetUserId();
        User user = userRepository.GetById(userId);

        if (!PasswordHelper.CheckPassword(model.OldPassword, user.PasswordHash))
        {
            ModelState.AddModelError("OldPassword", "Старый пароль введён неверно");
            return View(model);
        }

        string newHash = PasswordHelper.HashPassword(model.NewPassword);
        userRepository.UpdatePassword(userId, newHash);

        TempData["Success"] = "Пароль изменён!";
        return RedirectToAction("Index");
    }
}
