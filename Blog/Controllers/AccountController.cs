using Blog.Core.DTOs.Users.Commands;
using Blog.Core.Services.UserServices.Command;
using Blog.Core.Utils;
using Blog.Web.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Blog.Web.Controllers;

public class AccountController(IUserServiceCommand userServiceCommand) : BaseController
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

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register(RegisterUserViewModel vm)
    {
        if (ModelState.IsValid == false)
        {
            return View(vm);
        }

        var userDto = new RegisterUserDto
        {
            UserName = vm.UserName,
            Password = vm.Password,
            UserFullName = vm.UserFullName
        };

        var result = await userServiceCommand.RegisterUser(userDto);

        if (result.IsSuccess==false)
        {
            ModelState.AddModelError("UserName",result.Message);
            return View(vm);
        }

        TempData[TempDataName.ResultTempData] = JsonConvert.SerializeObject(result);

        return RedirectToAction(nameof(Login));
    }

    #endregion
}

