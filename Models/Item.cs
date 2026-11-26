using System.ComponentModel.DataAnnotations;

namespace BookShelf.Models;

public class Item
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Введите название")]
    [StringLength(200, ErrorMessage = "Название не длиннее 200 символов")]
    [Display(Name = "Название")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Введите автора")]
    [StringLength(100, ErrorMessage = "Автор не длиннее 100 символов")]
    [Display(Name = "Автор")]
    public string Author { get; set; }

    [Required(ErrorMessage = "Введите год")]
    [Range(1000, 2100, ErrorMessage = "Год должен быть от 1000 до 2100")]
    [Display(Name = "Год издания")]
    public int? Year { get; set; }

    [StringLength(2000, ErrorMessage = "Описание не длиннее 2000 символов")]
    [Display(Name = "Описание")]
    public string Description { get; set; }

    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }

    public string UserName { get; set; }
}
