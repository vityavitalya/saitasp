using System.ComponentModel.DataAnnotations;

namespace BookShelf.ViewModels;

public class ChangePasswordViewModel
{
    [Required(ErrorMessage = "Введите старый пароль")]
    [DataType(DataType.Password)]
    [Display(Name = "Старый пароль")]
    public string OldPassword { get; set; }

    [Required(ErrorMessage = "Введите новый пароль")]
    [MinLength(6, ErrorMessage = "Новый пароль должен быть минимум 6 символов")]
    [DataType(DataType.Password)]
    [Display(Name = "Новый пароль")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "Повторите новый пароль")]
    [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
    [DataType(DataType.Password)]
    [Display(Name = "Повторите новый пароль")]
    public string ConfirmNewPassword { get; set; }
}
