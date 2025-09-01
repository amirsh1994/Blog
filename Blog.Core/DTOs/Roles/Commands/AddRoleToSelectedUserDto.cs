namespace Blog.Core.DTOs.Roles.Commands;

public class AddRoleToSelectedUserDto
{
    public int UserId { get; set; }

    public List<int> RoleIds { get; set; } = [];
}