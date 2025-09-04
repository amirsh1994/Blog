using Blog.DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.DataBase.EntityMaps;

public class UserRoleEntityMap : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.HasIndex(x => new { x.UserId, x.RoleId })
            .IsUnique();

        builder.Property(x => x.CreationDate)
            .HasDefaultValueSql("GETDATE()");
    }
}