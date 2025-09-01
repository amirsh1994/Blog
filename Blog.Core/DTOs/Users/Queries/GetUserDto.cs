namespace Blog.Core.DTOs.Users.Queries;

public class GetUserDto
{
    public int UserId { get; set; }

    public string UserName { get; set; } = "";

    public string FullName { get; set; } = "";

    public string? UserImage { get; set; } = "";
}