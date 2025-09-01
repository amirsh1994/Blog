using System.ComponentModel.DataAnnotations;

namespace Blog.Web.Areas.Admin.Models.Roles;

public class CreateRoleViewModel
{
    [Display(Name = "نام نقش")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string RoleName { get; set; } = "";

    public List<int> PermissionIds { get; set; } = [];
}