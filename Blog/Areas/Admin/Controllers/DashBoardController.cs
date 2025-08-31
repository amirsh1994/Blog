using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Areas.Admin.Controllers;

[Authorize]
public class DashBoardController : BaseAdminController
{
    public IActionResult Index()
    {
        return View();
    }
}

