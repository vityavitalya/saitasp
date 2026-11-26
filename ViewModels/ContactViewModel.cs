using System.ComponentModel.DataAnnotations;

namespace BookShelf.ViewModels;

public class ContactViewModel
{
    [Required(ErrorMessage = "Введите имя")]
    [StringLength(50, ErrorMessage = "Имя не длиннее 50 символов")]
    [Display(Name = "Ваше имя")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Введите email")]
    [EmailAddress(ErrorMessage = "Неправильный формат email")]
    [StringLength(100, ErrorMessage = "Email не длиннее 100 символов")]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Напишите сообщение")]
    [StringLength(2000, MinimumLength = 10, ErrorMessage = "Сообщение должно быть от 10 до 2000 символов")]
    [Display(Name = "Сообщение")]
    public string Text { get; set; }
}
