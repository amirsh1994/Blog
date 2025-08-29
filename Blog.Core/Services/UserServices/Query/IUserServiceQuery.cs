using Blog.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace Blog.Core.Services.UserServices.Query;

public interface IUserServiceQuery
{
    Task<bool> ExistsUserName(string userName, int userId);
}

public class UserServiceQuery(BlogContext db) : IUserServiceQuery
{
    public async Task<bool> ExistsUserName(string userName, int userId)
    {
        var exists = await db.Users
            .AsNoTracking()
            .AnyAsync(x => x.UserName.Trim().ToLower() == userName && x.Id != userId);
        return exists;
    }
}