namespace Blog.DataBase.Entities;

public class User:BaseEntity
{
    public string UserName { get; set; } = "";

    public string FullName { get; set; } = "";

    public string Password { get; set; } = "";

    public string? UserImage { get; set; } = "";

    public List<Post> Posts { get; set; } = [];

    public List<Comment> Comments { get; set; } = [];

    public List<UserRole> UserRoles { get; set; } = [];
}

public class Role:BaseEntity
{
    public string Title { get; set; } = "";

    public List<RolePermission> RolePermissions { get; set; } = [];

    public List<UserRole> UserRoles { get; set; } = [];
}

public class UserRole:BaseEntity
{
    public int UserId { get; set; }

    public int RoleId { get; set; }

    public User User { get; set; }

    public Role Role { get; set; }

}

public class RolePermission : BaseEntity
{
    public int RoleId { get; set; }

    public int PermissionId { get; set; }

    public Role Role { get; set; }

    public Permission Permission { get; set; }
}

public class Permission:BaseEntity
{
    public string PermissionName { get; set; } = "";

    public List<RolePermission> RolePermissions { get; set; } = [];

}