using System.ComponentModel.DataAnnotations;

namespace BookShelf.ViewModels;

public class ProfileViewModel
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

    public DateTime CreatedAt { get; set; }
    public string Role { get; set; }
}
