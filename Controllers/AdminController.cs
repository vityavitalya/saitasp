using BookShelf.Models;
using BookShelf.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShelf.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    UserRepository userRepository = new UserRepository();
    ItemRepository itemRepository = new ItemRepository();
    MessageRepository messageRepository = new MessageRepository();

    public IActionResult Index()
    {
        List<User> users = userRepository.GetAll();
        return View(users);
    }

    public IActionResult Items()
    {
        List<Item> items = itemRepository.GetAll(null);
        return View(items);
    }

    [HttpPost]
    public IActionResult DeleteItem(int id)
    {
        itemRepository.DeleteAny(id);

        TempData["Success"] = "Книга удалена.";
        return RedirectToAction("Items");
    }

    public IActionResult Messages()
    {
        List<Message> messages = messageRepository.GetAll();
        return View(messages);
    }
}
