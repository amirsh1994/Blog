using Blog.Core.Services.HashServices;
using Blog.Core.Services.PermissionServices.Query;
using Blog.Core.Services.RoleService.Commands;
using Blog.Core.Services.RoleService.Queries;
using Blog.Core.Services.UserRoleServices.Command;
using Blog.Core.Services.UserRoleServices.Query;
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

        #region User_Role
        services.AddScoped<IUserRoleServiceCommand, UserRoleServiceCommand>();
        services.AddScoped<IUserRoleServiceQuery, UserRoleServiceQuery>();
        #endregion

        #region Hash
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        #endregion

        #region Role
        services.AddScoped<IRoleServiceQuery, RoleServiceQuery>();
        services.AddScoped<IRoleServiceCommand, RoleServiceCommand>();
        #endregion

        #region permission
        services.AddScoped<IPermissionServiceQuery, PermissionServiceQuery>();
        #endregion
    }
}