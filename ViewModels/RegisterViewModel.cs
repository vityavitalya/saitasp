using System.ComponentModel.DataAnnotations;

namespace BookShelf.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Введите имя")]
    [StringLength(50, ErrorMessage = "Имя не длиннее 50 символов")]
    [Display(Name = "Имя")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Введите email")]
    [EmailAddress(ErrorMessage = "Неправильный формат email")]
    [StringLength(100, ErrorMessage = "Email не длиннее 100 символов")]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Введите пароль")]
    [MinLength(6, ErrorMessage = "Пароль должен быть минимум 6 символов")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Повторите пароль")]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    [DataType(DataType.Password)]
    [Display(Name = "Повторите пароль")]
    public string ConfirmPassword { get; set; }
}
