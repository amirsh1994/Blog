namespace Blog.Core.DTOs.Roles.Commands;

public class UpdateRoleDto
{
    public int RoleId { get; set; }

    public string Title { get; set; } = "";

    public List<int>? PermissionIds { get; set; } = [];
}