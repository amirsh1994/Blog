using Blog.Bootstrap;
using Blog.Web.FileServices;

var builder = WebApplication.CreateBuilder(args);

#region Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.InitDependency(builder.Configuration.GetConnectionString("BlogTutorial") ?? "");
builder.Services.AddScoped<IFileService, FileService>();

#endregion

var app = builder.Build();

#region Configure the HTTP request pipeline.

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=DashBoard}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
#endregion

app.Run();
