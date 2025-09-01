namespace Blog.Web.Areas.Admin.Models.Users;

public class UpdateUserViewModel
{
    public int UserId { get; set; }

    public string UserName { get; set; } = "";

    public string FullName { get; set; } = "";

    public List<int> RoleIds { get; set; } = [];
}