namespace Blog.Core.DTOs.Users.Commands;

public class LoginDto
{
    public string UserName { get; set; } = "";

    public string Password { get; set; } = "";

    public string? ReturnUrl { get; set; }
}