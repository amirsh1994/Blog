using Blog.DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.DataBase.EntityMaps;

public class PermissionEntityMap:IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PermissionName)
            .HasMaxLength(100);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.HasMany(x=>x.RolePermissions)
            .WithOne(x=>x.Permission)
            .HasForeignKey(x=>x.PermissionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}