using System.ComponentModel.DataAnnotations;

namespace Blog.Web.Models.Users;

public class RegisterUserViewModel
{
    [Display(Name = "نام کاربری")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string UserName { get; set; } = "";

    [Display(Name = "رمز عبور")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string Password { get; set; } = "";

    [Display(Name = "نام و نام خانوادگی")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string UserFullName { get; set; } = "";
}