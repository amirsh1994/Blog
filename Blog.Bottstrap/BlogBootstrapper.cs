using Blog.DataBase.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Bootstrap;

public static class BlogBootstrapper
{
    public static void InitDependency(this IServiceCollection services, string connectionString)
    {
        #region context
        services.AddDbContext<BlogContext>(op =>op.UseSqlServer(connectionString),contextLifetime: ServiceLifetime.Scoped);
        #endregion



    }
}