using Blog.Bootstrap;
using Blog.Web.FileServices;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);


#region Add services to the container.
const string cookieName = "Blog";
builder.Services.AddControllersWithViews();
builder.Services.InitDependency(builder.Configuration.GetConnectionString("BlogTutorial") ?? "");
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(o =>
{
    o.Cookie.Name=cookieName;
    o.LoginPath = new PathString("/Login");
    o.AccessDeniedPath = new PathString("/Login");
    o.LogoutPath = new PathString("/LogOut");
});
builder.Services.AddHttpContextAccessor();

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
app.UseAuthentication();
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
