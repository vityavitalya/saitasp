using BookShelf.Models;
using BookShelf.Repositories;
using BookShelf.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookShelf.Controllers;

public class HomeController : Controller
{
    ItemRepository itemRepository = new ItemRepository();
    MessageRepository messageRepository = new MessageRepository();

    public IActionResult Index()
    {
        HomeViewModel model = new HomeViewModel();
        model.LatestItems = itemRepository.GetLatest(4);
        model.TotalCount = itemRepository.CountAll();

        return View(model);
    }

    public IActionResult About()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Contacts()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Contacts(ContactViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        Message message = new Message();
        message.Name = model.Name;
        message.Email = model.Email;
        message.Text = model.Text;
        messageRepository.Create(message);

        TempData["Success"] = "Спасибо! Ваше сообщение отправлено.";

        return RedirectToAction("Contacts");
    }

    public IActionResult PageNotFound()
    {
        return View();
    }
}
