using Blog.DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Blog.DataBase.Context;

public class BlogContext(DbContextOptions<BlogContext>options):DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        //foreach (IMutableForeignKey foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(x=>x.GetForeignKeys()))
        //{
        //    foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        //}
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<Comment> Comments { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Post> Posts { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<UserRole> UserRoles { get; set; }

    public DbSet<Permission> Permissions { get; set; }

    public DbSet<RolePermission> RolePermissions { get; set; }
}