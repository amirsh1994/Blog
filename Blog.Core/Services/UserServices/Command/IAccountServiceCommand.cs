using Blog.Core.Common;
using Blog.Core.DTOs.Users.Commands;
using Blog.Core.Services.HashServices;
using Blog.Core.Services.UserServices.Query;
using Blog.DataBase.Context;
using Blog.DataBase.Entities;

namespace Blog.Core.Services.UserServices.Command;

public interface IUserServiceCommand
{
    Task<OperationResult> RegisterUser(RegisterUserDto register);
}

public class UserServiceCommand(
     BlogContext db
    , IUserServiceQuery userServiceQuery
    , IPasswordHasher passwordHasher) : IUserServiceCommand
{
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
}