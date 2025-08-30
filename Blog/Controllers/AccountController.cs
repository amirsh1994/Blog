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
    public IActionResult Login(string? returnUrl = "")
    {
        var loginViewModel = new LoginViewModel()
        {
            ReturnUrl = returnUrl
        };
        return View(loginViewModel);
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login(LoginViewModel vm)
    {
        if (ModelState.IsValid == false)
        {
            return View();
        }
        var loginDto = new LoginDto
        {
            UserName = vm.UserName,
            Password = vm.Password,
            ReturnUrl = vm.ReturnUrl
        };
        var result = await userServiceCommand.Login(loginDto);
        TempData[TempDataName.ResultTempData] = JsonConvert.SerializeObject(result);
        if (!string.IsNullOrWhiteSpace(vm.ReturnUrl)&&Url.IsLocalUrl(vm.ReturnUrl))
        {
            Redirect(vm.ReturnUrl);
        }
        return View(vm);
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

        if (result.IsSuccess == false)
        {
            ModelState.AddModelError("UserName", result.Message);
            return View(vm);
        }

        TempData[TempDataName.ResultTempData] = JsonConvert.SerializeObject(result);

        return RedirectToAction(nameof(Login));
    }

    #endregion
}

