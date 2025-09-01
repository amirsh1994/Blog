using Blog.Core.Common;
using Blog.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace Blog.Core.Services.UserRoleServices.Query;

public interface IUserRoleServiceQuery
{
    Task<List<int>> GetUserRoleByIdAsync(int userId);

   
}

public class UserRoleServiceQuery (BlogContext db): IUserRoleServiceQuery
{
    public async Task<List<int>> GetUserRoleByIdAsync(int userId)
    {
        var userRoleIds = await db.UserRoles.AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x => x.RoleId).ToListAsync();
        return userRoleIds;
    }
}