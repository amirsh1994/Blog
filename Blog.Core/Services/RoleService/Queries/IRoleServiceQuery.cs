using Blog.Core.DTOs.Roles.Queries;
using Blog.Core.DTOs.Users.Queries;
using Blog.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace Blog.Core.Services.RoleService.Queries;

public interface IRoleServiceQuery
{
    Task<List<GetRoleDto>> GetRoles();

    Task<bool> ExistRoleName(string roleName, int roleId);
}

public class RoleServiceQuery(BlogContext db) : IRoleServiceQuery
{
    public async Task<List<GetRoleDto>> GetRoles()
    {
        var roles = await db.Roles.AsNoTracking()
            .Select(x => new GetRoleDto
            {
                RoleId = x.Id,
                RoleName = x.Title
            }).ToListAsync();
        return roles;
    }

    public async Task<bool> ExistRoleName(string roleName, int roleId)
    {
        var exists = await db.Roles.AnyAsync(x => x.Title == roleName.Trim().ToLower() && x.Id != roleId);
        return exists;
    }
}