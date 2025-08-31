using Blog.Core.Services.HashServices;
using Blog.Core.Services.RoleService.Commands;
using Blog.Core.Services.RoleService.Queries;
using Blog.Core.Services.UserServices.Command;
using Blog.Core.Services.UserServices.Query;
using Blog.DataBase.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Bootstrap;

public static class BlogBootstrapper
{
    public static void InitDependency(this IServiceCollection services, string connectionString)
    {
        #region context
        services.AddDbContext<BlogContext>(op => op.UseSqlServer(connectionString), contextLifetime: ServiceLifetime.Scoped);
        #endregion

        #region User

        services.AddScoped<IUserServiceCommand, UserServiceCommand>();
        services.AddScoped<IUserServiceQuery, UserServiceQuery>();

        #endregion

        #region Hash
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        #endregion

        #region Role
        services.AddScoped<IRoleServiceQuery, RoleServiceQuery>();
        services.AddScoped<IRoleServiceCommand, RoleServiceCommand>();
        #endregion
    }
}