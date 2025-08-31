using Blog.Core.Common;
using Blog.Core.DTOs.Roles.Commands;
using Blog.Core.Services.RoleService.Queries;
using Blog.DataBase.Context;
using Blog.DataBase.Entities;

namespace Blog.Core.Services.RoleService.Commands;

public interface IRoleServiceCommand
{
    Task<OperationResult> CreateRole(CreateRoleDto create);
}

public class RoleServiceCommand(BlogContext db,IRoleServiceQuery roleServiceQuery):IRoleServiceCommand
{
    public async Task<OperationResult> CreateRole(CreateRoleDto create)
    {
        if (await roleServiceQuery.ExistRoleName(create.RolName,0))
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
        return new OperationResult
        {
            IsSuccess = true,
            Message = OperationMessage.Create,
            Code = OperationCode.Success
        };
    }
}