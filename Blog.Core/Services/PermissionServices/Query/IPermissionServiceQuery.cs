using Blog.Core.DTOs.Permissions.Query;
using Blog.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace Blog.Core.Services.PermissionServices.Query;

public interface IPermissionServiceQuery
{
    Task<List<GetPermissionDto>> GetPermissions();

    Task<List<int>> GetPermissionIdsForSelectedRole(int roleId);
}


public class PermissionServiceQuery(BlogContext db) : IPermissionServiceQuery
{
    public async Task<List<GetPermissionDto>> GetPermissions()
    {
        var permissions = await db.Permissions.AsNoTracking()
            .Select(x => new GetPermissionDto
            {
                PermissionId = x.Id,
                PermissionName = x.PermissionName
            }).ToListAsync();
        return permissions;
    }

    public async Task<List<int>> GetPermissionIdsForSelectedRole(int roleId)
    {
        var permissionIds = await db.RolePermissions.AsNoTracking()
            .Where(x => x.RoleId == roleId)
            .Select(x => x.PermissionId).ToListAsync();
        return permissionIds;
    }
}