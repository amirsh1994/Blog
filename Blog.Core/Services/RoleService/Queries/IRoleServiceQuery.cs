using Blog.Core.DTOs.Roles.Queries;
using Blog.Core.DTOs.Users.Queries;
using Blog.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace Blog.Core.Services.RoleService.Queries;

public interface IRoleServiceQuery
{
    Task<List<GetRoleDto>> GetRoles();

    Task<bool> ExistRoleName(string roleName, int roleId);

    Task<List<int>> FindRolesForUser(int userId);

    Task<List<int>> FindRolesByPermissionId(int permissionId);

    Task<GetRoleDto?> GetRoleForUpdate(int roleId);
}

public class RoleServiceQuery(BlogContext db):IRoleServiceQuery
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

    public async Task<List<int>> FindRolesForUser(int userId)
    {
        var roleIds = await db.UserRoles.AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x => x.RoleId).ToListAsync();
        return roleIds;
    }

    public async Task<List<int>> FindRolesByPermissionId(int permissionId)
    {
        var rolIds = await db.RolePermissions.AsNoTracking()
            .Where(x => x.PermissionId == permissionId)
            .Select(x => x.RoleId).ToListAsync();
        return rolIds;
    }

    public async Task<GetRoleDto?> GetRoleForUpdate(int roleId)
    {
        var role = await db.Roles.AsNoTracking()
            .Where(x => x.Id == roleId)
            .Select(x => new GetRoleDto
            {
                RoleId = x.Id,
                RoleName = x.Title
            }).FirstOrDefaultAsync();
        return role;
    }
}