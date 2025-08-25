namespace Blog.DataBase.Entities;

public class Post:BaseEntity
{
    public string SmallTitle { get; set; } = "";

    public string Slug { get; set; } = "";

    public int VisitCount { get; set; }

    public string ImageName { get; set; } = "";

    public  string Description { get; set; } 

    public int UserId { get; set; }

    public User User { get; set; }

    public List<Comment> Comments { get; set; } = [];

    public int CategoryId { get; set; }

    public int ?SubCategoryId { get; set; }

    public Category Category { get; set; }

    public Category SubCategory { get; set; }

}