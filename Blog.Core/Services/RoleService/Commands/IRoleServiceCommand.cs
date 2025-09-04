using Blog.Core.Common;
using Blog.Core.DTOs.Roles.Commands;
using Blog.Core.Services.RoleService.Queries;
using Blog.Core.Utils;
using Blog.DataBase.Context;
using Blog.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.Core.Services.RoleService.Commands;

public interface IRoleServiceCommand
{
    Task<OperationResult> CreateRole(CreateRoleDto create);

    Task<OperationResult> AddRoleForSelectedUser(AddRoleToSelectedUserDto addRoleToSelectedUser);

    Task<OperationResult> RemoveRole(int roleId);

    Task<OperationResult>UpdateRole(UpdateRoleDto  updateRole);
}

public class RoleServiceCommand(BlogContext db, IRoleServiceQuery roleServiceQuery) : IRoleServiceCommand
{
    public async Task<OperationResult> CreateRole(CreateRoleDto create)
    {
        if (await roleServiceQuery.ExistRoleName(create.RolName, 0))
        {
            return new OperationResult
            {
                IsSuccess = false,
                Message = OperationMessage.Duplicated,
                Code = OperationCode.Duplicated
            };
        }

        var role = new Role
        {
            CreationDate = DateTime.Now,
            IsDeleted = false,
            Title = create.RolName,
        };

        db.Roles.Add(role);
        await db.SaveChangesAsync();

        if (role.Id < 0)
        {
            return new OperationResult
            {
                IsSuccess = false,
                Message = OperationMessage.Failed,
                Code = OperationCode.Failed
            };
        }

        List<RolePermission> rolePermissions = [];

        rolePermissions.AddRange(create.PermissionIds.Select(permissionId => new RolePermission
        {
            CreationDate = DateTime.Now,
            RoleId = role.Id,
            PermissionId = permissionId,
        }));
        db.RolePermissions.AddRange(rolePermissions);
        await db.SaveChangesAsync();

        return new OperationResult
        {
            IsSuccess = true,
            Message = OperationMessage.Create,
            Code = OperationCode.Success
        };
    }

    public async Task<OperationResult> AddRoleForSelectedUser(AddRoleToSelectedUserDto addRoleToSelectedUser)
    {
        try
        {
            List<UserRole> userRoles = [];

            userRoles.AddRange(addRoleToSelectedUser.RoleIds.Select(rolIds => new UserRole
            {
                CreationDate = DateTime.Now,
                UserId = addRoleToSelectedUser.UserId,
                RoleId = rolIds,
            }));
            db.UserRoles.AddRange(userRoles);
            await db.SaveChangesAsync();
            return new OperationResult
            {
                IsSuccess = true,
                Message = OperationMessage.Create,
                Code = OperationCode.Success
            };
        }
        catch (DbUpdateException ex)
        {
            return new OperationResult
            {
                IsSuccess = false,
                Message = "خطا: افزودن نقش تکراری برای کاربر",
                Code = OperationCode.Failed
            };
        }
    }

    public async Task<OperationResult> RemoveRole(int roleId)
    {
        var existingRole = await db.Roles.FirstOrDefaultAsync(x => x.Id == roleId);

        if (existingRole is null)
        {
            return new OperationResult
            {
                IsSuccess = false,
                Message = OperationMessage.NotFound,
                Code = OperationCode.NotFound
            };
        }
        existingRole.IsDeleted = true;
        await db.SaveChangesAsync();
        return new OperationResult
        {
            IsSuccess = true,
            Message = OperationMessage.Delete,
            Code = OperationCode.Success
        };

    }

    public async Task<OperationResult> UpdateRole(UpdateRoleDto updateRole)
    {
        var existingRole=await db.Roles.Include(x=>x.RolePermissions)
            .FirstOrDefaultAsync(x=>x.Id==updateRole.RoleId);

        if (existingRole is null)
        {
            return new OperationResult
            {
                IsSuccess = false,
                Message = OperationMessage.NotFound,
                Code = OperationCode.NotFound
            };
        }
        existingRole.LastModified=DateTime.Now;
        existingRole.Title = updateRole.Title;
        existingRole.SyncRolePermission(updateRole.PermissionIds ?? [],db);
        await db.SaveChangesAsync();
        return new OperationResult
        {
            IsSuccess = true,
            Message = OperationMessage.Update,
            Code = OperationCode.Success
        };
    }
}