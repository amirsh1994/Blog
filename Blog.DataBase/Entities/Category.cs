namespace Blog.DataBase.Entities;

public class Category : BaseEntity
{
    public string Title { get; set; } = "";

    public string Slug { get; set; } = "";

    public string MetaTag { get; set; } = "";

    public string MetaDescription { get; set; } = "";

    public List<Post> Posts { get; set; } = [];

    public List<Post> SubPost { get; set; } = [];

}