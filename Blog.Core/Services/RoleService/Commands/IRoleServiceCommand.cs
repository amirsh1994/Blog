using Blog.Core.Common;
using Blog.Core.DTOs.Roles.Commands;
using Blog.Core.Services.RoleService.Queries;
using Blog.DataBase.Context;
using Blog.DataBase.Entities;

namespace Blog.Core.Services.RoleService.Commands;

public interface IRoleServiceCommand
{
    Task<OperationResult> CreateRole(CreateRoleDto create);

    Task<OperationResult> AddRoleForSelectedUser(AddRoleToSelectedUserDto addRoleToSelectedUser);
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
        List<UserRole> userRoles = [];

        userRoles.AddRange(addRoleToSelectedUser.RoleIds.Select(rolIds=>new UserRole
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
}