namespace Blog.Core.DTOs.Users.Commands;

public class UpdateUserDto
{
    public int UserId { get; set; }

    public string UserName { get; set; } = "";

    public string FullName { get; set; } = "";

    public List<int> RolIds { get; set; } = [];
}