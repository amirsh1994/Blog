using Blog.Core.DTOs.Roles.Commands;
using Blog.Core.Services.RoleService.Commands;
using Blog.Core.Services.RoleService.Queries;
using Blog.Core.Utils;
using Blog.Web.Areas.Admin.Models.Roles;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Blog.Web.Areas.Admin.Controllers;

public class RoleController(IRoleServiceQuery roleServiceQuery,IRoleServiceCommand roleServiceCommand) : BaseAdminController
{
    #region Index

    public async Task<IActionResult> Index()
    {
        var roles = await roleServiceQuery.GetRoles();
        return View(roles);
    }

    #endregion

    #region create
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRoleViewModel vm)
    {
        if (ModelState.IsValid==false)
        {
            ModelState.AddModelError("RoleName","نام نقش نمیتواند خالی باشد");
            return View(vm);
        }
        var roleDto = new CreateRoleDto
        {
            RolName = vm.RoleName
        };
        var result = await roleServiceCommand.CreateRole(roleDto);
        TempData[TempDataName.ResultTempData] = JsonConvert.SerializeObject(result);
        return RedirectToAction(nameof(Index), "Role");
    }
    #endregion

}

