namespace Blog.DataBase.Entities;

public class Comment : BaseEntity
{
    public string Text { get; set; } = "";

    public int PostId { get; set; }

    public int UserId { get; set; }

    public Post Post { get; set; }

    public User User { get; set; }

}