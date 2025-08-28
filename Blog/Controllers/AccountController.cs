using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers;

public class AccountController:BaseController
{
    #region login
    [HttpGet]
    [Route("Login")]
    public IActionResult Login()
    {
        return View();
    }
    #endregion

    #region register

    [HttpGet]
    [Route("Register")]
    public IActionResult Register()
    {
        return View();
    }

    #endregion
}

