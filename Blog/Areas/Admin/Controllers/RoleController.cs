using Blog.Core.DTOs.Roles.Commands;
using Blog.Core.Services.PermissionServices.Query;
using Blog.Core.Services.RoleService.Commands;
using Blog.Core.Services.RoleService.Queries;
using Blog.Core.Services.UserRoleServices.Command;
using Blog.Core.Services.UserRoleServices.Query;
using Blog.Core.Utils;
using Blog.Web.Areas.Admin.Models.Roles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Blog.Web.Areas.Admin.Controllers;

public class RoleController(
       IRoleServiceQuery roleServiceQuery
       ,IRoleServiceCommand roleServiceCommand
       ,IPermissionServiceQuery permissionServiceQuery
       ,IUserRoleServiceQuery userRoleServiceQuery
       ,IUserRoleServiceCommand userRoleServiceCommand) : BaseAdminController
{
    #region Index

    public async Task<IActionResult> Index()
    {
        var roles = await roleServiceQuery.GetRoles();
        return View(roles);
    }

    #endregion

    #region create
    public async Task<IActionResult> Create()
    {
        await GetPermissions();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRoleViewModel vm)
    {
        if (ModelState.IsValid == false)
        {
            ModelState.AddModelError("RoleName", "نام نقش نمیتواند خالی باشد");
            return View(vm);
        }
        var roleDto = new CreateRoleDto
        {
            RolName = vm.RoleName,
            PermissionIds = vm.PermissionIds
        };
        var result = await roleServiceCommand.CreateRole(roleDto);
        TempData[TempDataName.ResultTempData] = JsonConvert.SerializeObject(result);
        return RedirectToAction(nameof(Index), "Role");
    }
    #endregion

    #region AddRoleToSelectedUser

    [HttpGet]
    [Route("AddRoleToSelectedUser/{userId:int}")]
    public async Task<IActionResult> AddRoleToSelectedUser(int userId)
    {
        var allRoles = await roleServiceQuery.GetRoles();
        ViewBag.Roles = allRoles;
        return View();
    }

    [HttpPost]
    [Route("AddRoleToSelectedUser/{userId:int}")]
    public async Task<IActionResult> AddRoleToSelectedUser(AddRoleToSelectedUserViewModel addRole)
    {
        var addRoleDto = new AddRoleToSelectedUserDto
        {
            UserId = addRole.UserId,
            RoleIds = addRole.RoleIds
        };
        var result = await roleServiceCommand.AddRoleForSelectedUser(addRoleDto);
        TempData[TempDataName.ResultTempData]= JsonConvert.SerializeObject(result);
        return RedirectToAction(nameof(Index));
    }

    #endregion

    #region GetPermissions
    private async Task GetPermissions()
    {
        var permissions = await permissionServiceQuery.GetPermissions();
        ViewBag.PermissionList = permissions;
    }
    #endregion

}

