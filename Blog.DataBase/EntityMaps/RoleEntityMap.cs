using Blog.DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.DataBase.EntityMaps;

public class RoleEntityMap:IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Title)
            .HasMaxLength(50);

        builder.HasMany(x=>x.UserRoles)
            .WithOne(x=>x.Role)
            .HasForeignKey(x=>x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x=>x.RolePermissions)
            .WithOne(x=>x.Role)
            .HasForeignKey(x=>x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}