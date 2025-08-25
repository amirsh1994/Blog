using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Areas.Admin.Controllers;

public class DashBoardController : BaseAdminController
{
    public IActionResult Index()
    {
        return View();
    }
}

