using Blog.Core.DTOs.Users.Commands;
using Blog.Core.Services.RoleService.Queries;
using Blog.Core.Services.UserRoleServices.Command;
using Blog.Core.Services.UserRoleServices.Query;
using Blog.Core.Services.UserServices.Command;
using Blog.Core.Services.UserServices.Query;
using Blog.Core.Utils;
using Blog.Web.Areas.Admin.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Blog.Web.Areas.Admin.Controllers;


public class UserController(
      IUserServiceQuery userServiceQuery
     ,IUserRoleServiceQuery userRoleServiceQuery
     ,IUserRoleServiceCommand userRoleServiceCommand
     ,IRoleServiceQuery roleServiceQuery
      ,IUserServiceCommand userServiceCommand):BaseAdminController
{
    #region Index

    public async Task<IActionResult> Index()
    {
        var users = await userServiceQuery.GetAll();
        return View(users);
    }

    #endregion

    #region Update
    [HttpGet]
    public async Task<IActionResult>Update(int id)
    {
        var allRoles = await roleServiceQuery.GetRoles();
        var userRoleIds = await userRoleServiceQuery.GetUserRoleByIdAsync(id);
        var getUserDto = await userServiceQuery.GetUserByUserIdAsync(id);
        ViewBag.AllRoles= allRoles;
        UpdateUserViewModel updateUserViewModel=new UpdateUserViewModel
        {
            UserId = id,
            UserName = getUserDto.UserName,
            FullName = getUserDto.FullName,
            RoleIds= userRoleIds
        };
        return View(updateUserViewModel);
    }

    [HttpPost]
    public async Task<IActionResult>Update(UpdateUserViewModel updateUserViewModel)
    {
        if (ModelState.IsValid==false)
        {
            return View(updateUserViewModel);
        }

        var updateDto = new UpdateUserDto
        {
            UserId = updateUserViewModel.UserId,
            UserName = updateUserViewModel.UserName,
            FullName = updateUserViewModel.FullName,
            RolIds = updateUserViewModel.RoleIds
        };
        var result = await userServiceCommand.UpdateUser(updateDto);
        TempData[TempDataName.ResultTempData] = JsonConvert.SerializeObject(result);
        return RedirectToAction(nameof(Index),"User");
    }
    #endregion

    #region Remove

    [HttpPost]
    public async Task<IActionResult>Remove(int id)
    {
        var result = await userServiceCommand.RemoveUser(id);
        TempData[TempDataName.ResultTempData] = JsonConvert.SerializeObject(result);
        return RedirectToAction(nameof(Index));
    }
    #endregion

}

