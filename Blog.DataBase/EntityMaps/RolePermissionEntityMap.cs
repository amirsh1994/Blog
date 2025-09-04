using Blog.DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.DataBase.EntityMaps;

public class RolePermissionEntityMap:IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.HasIndex(x=>new{x.RoleId,x.PermissionId})
            .IsUnique();

        builder.Property(x => x.CreationDate)
            .HasDefaultValueSql("GETDATE()");
    }
}