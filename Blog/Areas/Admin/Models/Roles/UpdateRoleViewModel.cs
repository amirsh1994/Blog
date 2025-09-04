namespace Blog.Web.Areas.Admin.Models.Roles;

public class UpdateRoleViewModel
{
    public int RoleId { get; set; }

    public string Title { get; set; } = "";

    public List<int>? PermissionIds { get; set; } = [];
}