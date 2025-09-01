using Blog.Core.Common;
using Blog.DataBase.Context;

namespace Blog.Core.Services.UserRoleServices.Command;

public interface IUserRoleServiceCommand
{
    Task<OperationResult> UpdateUserRolesAsync(int userId, List<int> roleIds);

    Task<OperationResult> AddRolesToUserAsync(int userId, List<int> roleIds);

    Task<OperationResult> RemoveRoleFromUserAsync(int userId, int roleId);
}

public class UserRoleServiceCommand(BlogContext db):IUserRoleServiceCommand
{
    public Task<OperationResult> UpdateUserRolesAsync(int userId, List<int> roleIds)
    {
        throw new NotImplementedException();
    }

    public Task<OperationResult> AddRolesToUserAsync(int userId, List<int> roleIds)
    {
        throw new NotImplementedException();
    }

    public Task<OperationResult> RemoveRoleFromUserAsync(int userId, int roleId)
    {
        throw new NotImplementedException();
    }
}