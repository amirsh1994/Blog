using System.Security.Claims;
using Blog.Core.DTOs.Users.Queries;
using Blog.Core.Utils;
using Blog.DataBase.Context;
using Blog.DataBase.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Blog.Core.Services.UserServices.Query;

public interface IUserServiceQuery
{
    Task<bool> ExistsUserName(string userName, int userId);

    Task<User?> GetUserByUserName(string userName);

    GetCurrentUserDto GetCurrentUser();
}

public class UserServiceQuery(BlogContext db, IHttpContextAccessor contextAccessor) : IUserServiceQuery
{
    public async Task<bool> ExistsUserName(string userName, int userId)
    {
        var exists = await db.Users
            .AsNoTracking()
            .AnyAsync(x => x.UserName.Trim().ToLower() == userName && x.Id != userId);
        return exists;
    }

    public async Task<User?> GetUserByUserName(string userName)
    {
        var user = await db.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        return user;
    }

    public GetCurrentUserDto GetCurrentUser()
    {
        var httpContext = contextAccessor.HttpContext;

        if (httpContext == null || httpContext.User?.Identity?.IsAuthenticated != true)
            return new GetCurrentUserDto();

        var user = httpContext.User;

        var userIdValue = user.FindFirstValue(ClaimEx.UserId);

        if (!int.TryParse(userIdValue, out var userId))
            return new GetCurrentUserDto();

        var userName = user.FindFirstValue(ClaimEx.UserName);

        return new GetCurrentUserDto
        {
            UserId = userId,
            UserName = userName ?? ""
        };
    }
}