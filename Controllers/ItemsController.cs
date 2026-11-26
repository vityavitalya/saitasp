using BookShelf.Models;
using BookShelf.Repositories;
using BookShelf.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookShelf.Controllers;

public class ItemsController : Controller
{
    ItemRepository itemRepository = new ItemRepository();

    public IActionResult Index(string search)
    {
        List<Item> items = itemRepository.GetAll(search);

        ViewBag.Search = search;
        return View(items);
    }

    public IActionResult Details(int id)
    {
        Item item = itemRepository.GetById(id);

        if (item == null)
        {
            return NotFound();
        }

        return View(item);
    }

    public IActionResult Authors()
    {
        List<AuthorViewModel> authors = itemRepository.GetAuthors();
        return View(authors);
    }
}
