using System.ComponentModel.DataAnnotations;

namespace BookShelf.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Введите email")]
    [EmailAddress(ErrorMessage = "Неправильный формат email")]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Введите пароль")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; }
}
