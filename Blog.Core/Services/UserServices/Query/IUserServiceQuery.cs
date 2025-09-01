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

    Task<List<GetUserDto>> GetAll();

    Task<GetUserDto?> GetUserByUserIdAsync(int userId);

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

    public async Task<GetUserDto?> GetUserByUserIdAsync(int userId)
    {
        var getUser = await db.Users.AsNoTracking()
            .Where(x => x.Id == userId)
            .Select(x => new GetUserDto
            {
                UserId = x.Id,
                UserName = x.UserName,
                FullName = x.FullName,
                UserImage = x.UserImage
            }).FirstOrDefaultAsync();
        return getUser;
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

    public async Task<List<GetUserDto>> GetAll()
    {
        var users = await db.Users.AsNoTracking()
            .Select(x => new GetUserDto
            {
                UserId = x.Id,
                UserName = x.UserName,
                FullName = x.FullName,
                UserImage = x.UserImage
            }).ToListAsync();
        return users;
    }
}