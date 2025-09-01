namespace Blog.Web.Areas.Admin.Models.Roles;

public class AddRoleToSelectedUserViewModel
{
    public int UserId { get; set; }

    public List<int> RoleIds { get; set; } = [];


}