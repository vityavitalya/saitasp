using BookShelf.Models;

namespace BookShelf.ViewModels;

public class HomeViewModel
{
    public List<Item> LatestItems { get; set; }
    public int TotalCount { get; set; }
}
