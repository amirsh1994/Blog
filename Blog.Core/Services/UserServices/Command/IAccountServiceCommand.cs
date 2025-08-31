using System.Security.Claims;
using Blog.Core.Common;
using Blog.Core.DTOs.Users.Commands;
using Blog.Core.Services.HashServices;
using Blog.Core.Services.UserServices.Query;
using Blog.Core.Utils;
using Blog.DataBase.Context;
using Blog.DataBase.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace Blog.Core.Services.UserServices.Command;

public interface IUserServiceCommand
{
    Task<OperationResult> RegisterUser(RegisterUserDto register);

    Task<OperationResult> LogOut();

    Task<OperationResult> Login(LoginDto loginDto);
}

public class UserServiceCommand(BlogContext db,IUserServiceQuery userServiceQuery,IPasswordHasher passwordHasher,IHttpContextAccessor accessor):IUserServiceCommand
{
    #region Register
    public async Task<OperationResult> RegisterUser(RegisterUserDto register)
    {

        if (await userServiceQuery.ExistsUserName(register.UserName, 0))
        {
            return new OperationResult
            {
                IsSuccess = false,
                Message = OperationMessage.Duplicated,
                Code = OperationCode.Duplicated
            };
        }

        string hashPassword = passwordHasher.Hash(register.Password);


        var user = new User
        {
            CreationDate = DateTime.Now,
            UserName = register.UserName,
            FullName = register.UserFullName,
            Password = hashPassword,
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return new OperationResult
        {
            IsSuccess = true,
            Message = OperationMessage.Register,
            Code = OperationCode.Success
        };
    }
    #endregion

    #region Logout
    public async Task<OperationResult> LogOut()
    {
        await accessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return new OperationResult
        {
            IsSuccess = true,
            Message = "با موفقیت از سیستم خارج شدید",
            Code = OperationCode.Success
        };
    }
    #endregion

    #region Login

    public async Task<OperationResult> Login(LoginDto loginDto)
    {
        var findUser = await userServiceQuery.GetUserByUserName(loginDto.UserName);

        if (findUser is null)
        {
            return new OperationResult
            {
                IsSuccess = false,
                Message = OperationMessage.NotFoundUser,
                Code = OperationCode.Failed
            };
        }
        var (verified,needsUpgrade)=await passwordHasher.CheckAsync(findUser.Password, loginDto.Password);

        if (verified is false)
        {
            return new OperationResult
            {
                IsSuccess = false,
                Message = OperationMessage.InvalidCredential,
                Code = OperationCode.Failed
            };
        }
        var userClaims = new List<Claim>()
        {
            new(ClaimEx.UserId,findUser.Id.ToString()),
            new(ClaimEx.UserName,findUser.UserName)
        };
        var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent =true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),
            AllowRefresh = true,
        };

        await accessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

        return new OperationResult
        {
            IsSuccess = true,
            Message = OperationMessage.LoginSuccess,
            Code = OperationCode.Success
        };
    }

    #endregion
}